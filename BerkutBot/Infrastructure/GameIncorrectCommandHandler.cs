using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Infrastructure
{
	public class GameIncorrectCommandHandler : IGameAnswer
	{
        private const string REPLY_TEXT = "Смахивает на бред :(";
        private readonly ITelegramBotClient _telegramBotClient;

        public GameIncorrectCommandHandler(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public int Order => 999;

        public Func<string, bool> Intent => (string text) => true;

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

