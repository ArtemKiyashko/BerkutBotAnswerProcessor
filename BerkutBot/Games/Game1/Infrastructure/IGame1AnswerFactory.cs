using System;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game1.Infrastructure
{
    public interface IGame1AnswerFactory
    {
        public IGame1Answer GetInstance(Message message);
    }
}

