using System;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game5.StartCommands
{
	public class Point5 : IStartCommand
	{
        private const string ANSWER = "Point5_a796ec50-7548-4b61-b850-735aafc447b0";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point5> _logger;

        public Point5(ITelegramBotClient telegramBotClient, ILogger<Point5> logger)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 5;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendLocationAsync(message.Chat.Id, 60.021008, 29.688790);
            return $"{ANSWER} coordinates sent";
        }
    }
}

