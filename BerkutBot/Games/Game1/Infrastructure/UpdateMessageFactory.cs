using System;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game1.Infrastructure
{
    public class UpdateMessageFactory : IUpdateMessageFactory
    {

        public Message GetMessage(Update incomingUpdate) => incomingUpdate?.Type switch
        {
            Telegram.Bot.Types.Enums.UpdateType.Message => incomingUpdate.Message,
            Telegram.Bot.Types.Enums.UpdateType.EditedMessage => incomingUpdate.EditedMessage,
            _ => null
        };
    }
}

