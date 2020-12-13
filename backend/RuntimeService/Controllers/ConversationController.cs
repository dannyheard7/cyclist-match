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
    [Route("conversations")]
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
        
        private class ConversationWithLastMessageResult
        {
            public ConversationWithLastMessageResult(ICollection<Profile> userProfiles, Message lastMessage)
            {
                UserProfiles = userProfiles;
                LastMessage = lastMessage;
            }

            public ICollection<Profile> UserProfiles { get; }
            public Message LastMessage { get; }
        }
        
        private class ConversationsResult
        {
            public ConversationsResult(IEnumerable<ConversationWithLastMessageResult> conversations)
            {
                Conversations = conversations;
            }

            public IEnumerable<ConversationWithLastMessageResult> Conversations { get; }
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
            
            List<ConversationWithLastMessageResult> results = new List<ConversationWithLastMessageResult>();

            // TODO: this can be way more efficient
            foreach (var conversation in conversations)
            {
                var otherUser = conversation.Users.First(user => user.Id != currentUser.Id);
                var otherUserProfile = await _profileService.Get(otherUser.Id!.Value);
                var lastMessageConvoResult = new ConversationWithLastMessageResult(new List<Profile>{ otherUserProfile! }, conversation.Messages.First());
                results.Add(lastMessageConvoResult);
            }
            return Ok(new ConversationsResult(results));
        }
        
        private class ConversationResult
        {
            public ConversationResult(IEnumerable<Profile> userProfiles, IEnumerable<Message> messages)
            {
                UserProfiles = userProfiles;
                Messages = messages;
            }

            public IEnumerable<Profile> UserProfiles { get; }
            public IEnumerable<Message> Messages { get; }
        }
        
        [HttpGet("{conversationId}")]
        public async Task<IActionResult> GetConversation(Guid conversationId)
        {
            var currentUser = await _currentUserService.GetUser();
            
            var conversation = await _messageService.GetConversationById(conversationId, currentUser, 10);
            if (conversation == null) return NotFound();

            await _messageService.MarkUnreadMessagesAsRead(conversation, currentUser);
            
            var otherUser = conversation.Users.First(user => user.Id != currentUser.Id);
            var otherUserProfile = await _profileService.Get(otherUser.Id!.Value) ?? throw new Exception();
            
            return Ok(new ConversationResult(new List<Profile>{ otherUserProfile }, conversation.Messages));
        }
        
        public class CreateConversationInput
        {
            public Guid UserId { get; set; }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateConversation([FromBody] CreateConversationInput createConversationInput)
        {
            var currentUser = await _currentUserService.GetUser();
            var otherUser = await _userService.GetUserById(createConversationInput.UserId) ??
                            throw new Exception("Could not find user");
            
            var conversation = new Conversation(Guid.NewGuid(), new List<IUser>{ currentUser, otherUser });
            await _messageService.CreateConversation(conversation);

            return Ok(conversation);
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