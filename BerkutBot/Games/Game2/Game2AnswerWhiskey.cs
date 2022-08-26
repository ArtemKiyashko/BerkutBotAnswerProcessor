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
	public class Game2AnswerWhiskey : IGameAnswer
	{
        private const string REPLY_TEXT = "A bheil thu fhathast an seo, poca leathair? Coimhead airson cruth-atharrachaidh faisg air a’ chiad taigh!";
        private readonly HashSet<string> _answerSet = new() { "whiskey", "виски" };

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Game2AnswerWhiskey> _logger;

        public Game2AnswerWhiskey(ITelegramBotClient telegramBotClient, ILogger<Game2AnswerWhiskey> logger)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
        }

        public Func<string, bool> Intent =>
            text =>
            _answerSet.Any(ans => ans.Equals(text, StringComparison.OrdinalIgnoreCase));

        public int Order => 4;

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
                _logger.LogError($"Cannot send whiskey answer: {ex.Message}", ex);
                return $"Cannot send whiskey answer: {ex.Message}";
            }
            return REPLY_TEXT;
        }
    }
}

