using System;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game4.StartCommands
{
    public class UnknownStartCommand : IStartCommand
    {
        private const string REPLY_TEXT = "Сейчас нет активных игр. Мы дадим вам знать, когда начнется следующая. Оставайтесь на связи и не удаляйте этот чат, чтобы быть в курсе событий ;-)";
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

