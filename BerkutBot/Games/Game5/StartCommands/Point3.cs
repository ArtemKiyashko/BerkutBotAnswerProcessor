using System;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game5.StartCommands
{
	public class Point3 : IStartCommand
	{
        private const string ANSWER = "Point3_4057b4cc-b708-482a-b8f7-9214b5a28cc9";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point3> _logger;

        public Point3(ITelegramBotClient telegramBotClient, ILogger<Point3> logger)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 3;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendLocationAsync(message.Chat.Id, 59.89875, 29.86706);
            return $"{ANSWER} coordinates sent";
        }
    }
}

