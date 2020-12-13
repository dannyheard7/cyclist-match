using System;
using System.Collections.Generic;

namespace Persistence.Entity
{
    public class Conversation
    {
        public Conversation(Guid id, IEnumerable<IUser> users, IEnumerable<Message>? messages = null)
        {
            Id = id;
            Users = users;
            Messages = messages ?? new List<Message>();
        }

        public Guid Id { get; }
        public IEnumerable<IUser> Users { get; }
        public IEnumerable<Message> Messages { get; }
    }
}