using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatService;
using ChatService.Models;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Messaging.Types;
using RuntimeService.Controllers.Models;
using RuntimeService.Services;

namespace RuntimeService.Controllers;

[ApiController]
[Route("api/conversations")]
[Authorize]
public class MessagingController
{
    private readonly IChatClient _chatClient;
    private readonly IUserContext _userService;

    public MessagingController(
        IChatClient chatClient,
        IUserContext userService)
    {
        _chatClient = chatClient;
        _userService = userService;
    }

    [HttpGet]
    public async Task<Page<Conversation>> GetConversations(
        [FromQuery(Name = "unread")] bool unread = false,
        [FromQuery(Name = "pageSize")] int pageSize = 15,
        [FromQuery(Name="page")] int? page = 0
        )
    {
        var currentUser = await _userService.GetUserProfile();

        var pageRequest = new PageRequest(pageSize, page);
        return await _chatClient.GetUserConversations(currentUser.UserId, pageRequest, unread);
    }

    [HttpGet("byUsers")]
    public async Task<Conversation?> GetConversation([FromQuery(Name = "userId")] IReadOnlyCollection<Guid> userIds)
    {
        var currentUser = await _userService.GetUserProfile();

        var allIds = new HashSet<Guid>(userIds) { currentUser.UserId };
        return await _chatClient.GetConversationBetweenUsers(allIds);
    }

    [HttpPost("message")]
    public async Task<MessageDTO> SendMessage([FromBody] MessageInput input)
    {
        var sender = await _userService.GetUserProfile();
        return await _chatClient.SendMessage(sender.UserId, input.Recipients, input.Body);
    }
}