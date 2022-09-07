using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game2
{
	public class Game2AnswerWarsawExpress : IGameAnswer
	{
        private const string REPLY_TEXT = "Верный ответ! Ждём вас на второй этап BerkutCityAutoQuest 9 сентября. Время: 19:30. Место: парковка Варшавского экспресса";
        private readonly HashSet<string> _answerSet = new() { "Варшавский экспресс" };

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Game2AnswerWarsawExpress> _logger;

        public Game2AnswerWarsawExpress(ITelegramBotClient telegramBotClient, ILogger<Game2AnswerWarsawExpress> logger)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
        }

        public Func<string, bool> Intent =>
            text =>
            _answerSet.Any(ans => ans.Equals(text, StringComparison.OrdinalIgnoreCase));

        public int Order => 5;

        public async Task<string> Reply(Message message)
        {

            try
            {
                await _telegramBotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: REPLY_TEXT,
                    replyToMessageId: message.MessageId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot send WarsawExpress answer: {ex.Message}", ex);
                return $"Cannot send WarsawExpress answer: {ex.Message}";
            }
            return REPLY_TEXT;
        }
    }
}

