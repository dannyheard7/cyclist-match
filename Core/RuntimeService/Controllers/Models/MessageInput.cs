using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace RuntimeService.Controllers.Models;

public class MessageInput
{
    public MessageInput(ImmutableHashSet<Guid> recipients, string body)
    {
        Recipients = recipients;
        Body = body;
    }
    
    public ImmutableHashSet<Guid> Recipients { get; }

    public string Body { get; }
}