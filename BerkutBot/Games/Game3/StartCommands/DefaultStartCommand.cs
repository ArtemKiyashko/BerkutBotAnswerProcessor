using System;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game3.StartCommands
{
    public class DefaultStartCommand : IStartCommand
    {
        private const string REPLY_TEXT = "Короче, {0}, я тебя спас и в благородство играть не буду: выполнишь для меня пару заданий — и мы в расчете." +
            "\nЗаодно посмотрим, как быстро у тебя башка после амнезии прояснится. А по твоей теме постараюсь разузнать." +
            "\nХрен его знает, на кой ляд тебе этот Беркут сдался, но скоро ты узнаешь, откуда начнется приключение...";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<DefaultStartCommand> _logger;

        public DefaultStartCommand(ITelegramBotClient telegramBotClient, ILogger<DefaultStartCommand> logger)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
        }

        public Func<string, bool> Intent => (string text) => string.IsNullOrEmpty(text);

        public int Order => 999;

        public async Task<string> Reply(Message message)
        {
            try
            {
                var replyFormatted = string.Format(REPLY_TEXT, message.From.FirstName ?? message.From.Username);
                await _telegramBotClient.SendTextMessageAsync(
                    message.Chat.Id,
                    text: replyFormatted);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DefaultStartCommand)} fails: {ex.Message}", ex);
                return $"{nameof(DefaultStartCommand)} fails: {ex.Message}";
            }
            return REPLY_TEXT;
        }
    }
}

