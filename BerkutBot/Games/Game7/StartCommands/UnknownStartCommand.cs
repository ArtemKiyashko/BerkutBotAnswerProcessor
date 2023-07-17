using System;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game7.StartCommands
{
    public class UnknownStartCommand : IStartCommand
    {
        private const string REPLY_TEXT = "Ну бред жи, ну?";
        private readonly ITelegramBotClient _telegramBotClient;

        public UnknownStartCommand(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public Func<string, bool> Intent => (string text) => true;

        public int Order => 1000;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: REPLY_TEXT,
                replyToMessageId: message.MessageId);
            return REPLY_TEXT;
        }
    }
}

