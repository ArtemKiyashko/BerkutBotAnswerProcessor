using System;
using System.Threading.Tasks;
using BerkutBot.Games.Game3.StartCommands;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game5.StartCommands
{
	public class Point1 : IStartCommand
	{
        private const string ANSWER = "Point1_7aaa6cfc-bc2e-4b2c-9b59-88c62d52072f";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point1> _logger;

        public Point1(ITelegramBotClient telegramBotClient, ILogger<Point1> logger)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 1;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendLocationAsync(message.Chat.Id, 59.86052, 30.04752);
            return $"{ANSWER} coordinates sent";
        }
    }
}

