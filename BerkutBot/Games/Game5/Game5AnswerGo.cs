using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game5
{
    public class Game5AnswerGo : IGameAnswer
    {
        private const string REPLY_TEXT = ",fyrtnysq pfk z[n-rke,";

        private readonly HashSet<string> _answerSet = new() { "поехали", "poehali" };
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Game5AnswerGo> _logger;

        public Game5AnswerGo(ITelegramBotClient telegramBotClient, ILogger<Game5AnswerGo> logger)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
        }

        public Func<string, bool> Intent =>
            text =>
            _answerSet.Any(ans => ans.Equals(text, StringComparison.OrdinalIgnoreCase));

        public int Order => 1;

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

