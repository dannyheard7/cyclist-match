using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Entity;
using RuntimeService.DTO;
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
        
        public ConversationController(ICurrentUserService currentUserService, IMessageService messageService)
        {
            _currentUserService = currentUserService;
            _messageService = messageService;
        }
        
        private class ConversationsResult
        {
            public ConversationsResult(IEnumerable<Conversation> conversations)
            {
                Conversations = conversations;
            }

            public IEnumerable<Conversation> Conversations { get; }
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
            return Ok(new ConversationsResult(conversations));
        }
        
    }
}