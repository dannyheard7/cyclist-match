using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Persistence.Entity;
using RuntimeService.Services;

namespace RuntimeService.Controllers
{
    [ApiController]
    [Route("api/conversations")]
    [Authorize]
    public class ConversationController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMessageService _messageService;
        private readonly IProfileService _profileService;
        private readonly IUserService _userService;
        
        public ConversationController(ICurrentUserService currentUserService, IUserService userService, IMessageService messageService, IProfileService profileService)
        {
            _currentUserService = currentUserService;
            _userService = userService;
            _messageService = messageService;
            _profileService = profileService;
        }

        private class ConversationsResult
        {
            public ConversationsResult(IEnumerable<ConversationResult> conversationResults)
            {
                Conversations = conversationResults;
            }

            public IEnumerable<ConversationResult> Conversations { get; }
        }
        
        [HttpGet("unread/count")]
        public async Task<IActionResult> GetCountUnreadConversations()
        {
            var currentUser = await _currentUserService.GetUser();
            var numberConversations = await _messageService.GetNumberConversationsWithUnreadMessages(currentUser);
            return Ok(numberConversations);
        }

        [HttpGet]
        public async Task<IActionResult> GetConversations()
        {
            var currentUser = await _currentUserService.GetUser();
            var conversations = await _messageService.GetUserConversations(currentUser);
            
            List<ConversationResult> results = new List<ConversationResult>();

            // TODO: this can be way more efficient
            foreach (var conversation in conversations)
            {
                var otherUser = conversation.Users.First(user => user.Id != currentUser.Id);
                var otherUserProfile = await _profileService.Get(otherUser.Id!.Value);
                results.Add(new ConversationResult(new List<Profile>{ otherUserProfile! }, conversation));
            }
            return Ok(new ConversationsResult(results));
        }
        
        private class ConversationResult
        {
            public ConversationResult(IEnumerable<Profile> userProfiles, Conversation conversation)
            {
                Id = conversation.Id;
                UserProfiles = userProfiles;
                Messages = conversation.Messages;
            }
            
            public Guid Id { get; }
            public IEnumerable<Profile> UserProfiles { get; }
            public IEnumerable<Message> Messages { get; }
        }
        
        [HttpGet("users")]
        public async Task<IActionResult> GetConversationBetweenUsers([FromQuery(Name = "id")] IEnumerable<Guid> userIds)
        {
            var currentUser = await _currentUserService.GetUser();
            var getUsersTasks = userIds.Select(id => _userService.GetUserById(id)).ToList();
            await Task.WhenAll(getUsersTasks);
            var users = getUsersTasks.Where(x => x.Result != null).Select(x => x.Result);

            var allusers = new List<IUser>(users);
            allusers.Add(currentUser);
            var conversation = await _messageService.GetConversationBetweenUsers(allusers, 10);
            
            if (conversation == null)
            {
                conversation = new Conversation(Guid.NewGuid(), allusers);
                await _messageService.CreateConversation(conversation);
            }

            await _messageService.MarkUnreadMessagesAsRead(conversation, currentUser);
            
            List<Profile> profiles = new List<Profile>();
            foreach (var otherUser in users)
            {
                var otherUserProfile = await _profileService.Get(otherUser.Id!.Value);
                profiles.Add(otherUserProfile);
            }
            
            return Ok(new ConversationResult(profiles, conversation));
        }

        public class MessageInput
        {
            public string Message { get; set; }
        }
        
        [HttpPost("{conversationId}/message")]
        public async Task<IActionResult> SendMessageInConversation(Guid conversationId, [FromBody] MessageInput messageInput)
        {
            var currentUser = await _currentUserService.GetUser();
            var conversation = await _messageService.GetConversationById(conversationId, currentUser, 0);
            if (conversation == null) return NotFound();
            
            var message = Message.Create(currentUser, messageInput.Message);
            await _messageService.SendMessage(conversation, message);
            return NoContent();
        }
        
    }
}