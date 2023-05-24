using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game5.StartCommands
{
	public class Point6 : IStartCommand
	{
        private const string ANSWER = "Point6_b9913729-3ef7-4760-9f48-fc4e745f551f_NOT_READY";
        private const string ANSWER_VIDEO_URL = "https://sawevprivate.blob.core.windows.net/public/Game5/point4_pic.jpeg";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point6> _logger;

        public Point6(
            ITelegramBotClient telegramBotClient,
            ILogger<Point6> logger)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 6;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendVideoAsync(message.Chat.Id, InputFile.FromUri(ANSWER_VIDEO_URL));
            return $"{ANSWER} coordinates sent";
        }
    }
}

