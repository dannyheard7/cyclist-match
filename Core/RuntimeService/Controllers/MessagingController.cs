using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatService;
using ChatService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Messaging.Types;
using RuntimeService.Controllers.Models;
using RuntimeService.Services;

namespace RuntimeService.Controllers;

[ApiController]
[Route("api/messages")]
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

    [HttpPost]
    public async Task<MessageDTO> SendMessage([FromBody] MessageInput input)
    {
        var sender = await _userService.GetUserProfile();
        return await _chatClient.SendMessage(sender.UserId, new HashSet<Guid> { input.RecipientId }, input.Body);
    }

    [HttpGet]
    public async Task<Conversation> GetConversation([FromQuery(Name = "userId")] IReadOnlyCollection<Guid> userIds)
    {
        var currentUser = await _userService.GetUserProfile();

        var allIds = new HashSet<Guid>(userIds) { currentUser.UserId };
        return await _chatClient.GetConversationBetweenUsers(allIds);
    }
}