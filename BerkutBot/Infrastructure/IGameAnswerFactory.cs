using System;
using Telegram.Bot.Types;

namespace BerkutBot.Infrastructure
{
    public interface IGameAnswerFactory
    {
        public IGameAnswer GetInstance(Message message);
    }
}

