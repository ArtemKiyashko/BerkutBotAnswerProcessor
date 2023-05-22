using System;
using System.Threading.Tasks;
using BerkutBot.Games.Game3.StartCommands;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game5.StartCommands
{
	public class Point2 : IStartCommand
	{
        private const string ANSWER = "Point2_d0bdfb55-4c60-4323-816c-441ce7bb791a";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point2> _logger;

        public Point2(ITelegramBotClient telegramBotClient, ILogger<Point2> logger)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 2;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendLocationAsync(message.Chat.Id, 59.84886, 30.00002);
            return $"{ANSWER} coordinates sent";
        }
    }
}

