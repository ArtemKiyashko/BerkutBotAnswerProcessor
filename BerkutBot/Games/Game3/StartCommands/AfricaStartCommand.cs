using System;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game3.StartCommands
{
    public class AfricaStartCommand : IStartCommand
    {
        private const string REPLY_TEXT = "Правильно! Получите следующее задание!";
        private const string ANSWER = "africa";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<DefaultStartCommand> _logger;

        public AfricaStartCommand(ITelegramBotClient telegramBotClient, ILogger<DefaultStartCommand> logger)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

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
                _logger.LogError($"{nameof(AfricaStartCommand)} fails: {ex.Message}", ex);
                return $"{nameof(AfricaStartCommand)} fails: {ex.Message}";
            }
            return REPLY_TEXT;
        }
    }
}

