using System;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game5.StartCommands
{
    public class DefaultStartCommand : IStartCommand
    {
        private const string REPLY_TEXT = "Привет {0}! В этот раз тебе понадобится телефон с NFC, которым тебе придется сканировать найденные NFC-метки.\n" +
            "Каждая метка содержит в себе ссылку (но ты ее не увидишь), которую я могу понять и <s>простить</s> прочесть.\n" +
            "Если ты не можешь найти метку - не беда! " +
            "Обратись к организаторам и они пришлют тебе специальную ссылку - ткни на нее и я засчитаю тебе прохождение точки.";

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
            var replyFormatted = string.Format(REPLY_TEXT, message.From.FirstName ?? message.From.Username);
            await _telegramBotClient.SendTextMessageAsync(
                message.Chat.Id,
                text: replyFormatted,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

            return REPLY_TEXT;
        }
    }
}

