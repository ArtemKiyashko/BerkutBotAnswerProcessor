using System;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game1.Infrastructure
{
    public interface IUpdateMessageFactory
    {
        public Message GetMessage(Update incomingUpdate);
    }
}

