using System;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game2
{
	public class Game2AnswerIncorrect : IGameAnswer
	{
        private const string REPLY_TEXT = "Если на необитаемом острове упало бы дерево, издавался ли там звук? Не знаю. Как и ответа на твой вопрос. Попробуй что-то другое.";
        private readonly ITelegramBotClient _telegramBotClient;

        public Game2AnswerIncorrect(ITelegramBotClient telegramBotClient)
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

