using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game1
{
    public interface IGame1Answer
    {
        public Func<string, bool> Intent { get; }
        public int Order { get; }
        public Task<string> Reply(Message message);
    }
}

