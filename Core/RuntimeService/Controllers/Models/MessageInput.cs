using System;
using System.Collections.Generic;

namespace RuntimeService.Controllers.Models;

public class MessageInput
{
    public MessageInput(IReadOnlySet<Guid> recipients, string body)
    {
        Recipients = recipients;
        Body = body;
    }
    
    public IReadOnlySet<Guid> Recipients { get; }

    public string Body { get; }
}