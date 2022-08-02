using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BerkutBot.Infrastructure
{
    public interface IGameAnswer
    {
        public Func<string, bool> Intent { get; }
        public int Order { get; }
        public Task<string> Reply(Message message);
    }
}

