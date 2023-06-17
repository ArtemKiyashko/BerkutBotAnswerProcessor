using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BerkutBot.Infrastructure;
using BerkutBot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BerkutBot.Games.Game6.StartCommands
{
	public class Point12 : IStartCommand
	{
        private const string ANSWER = "Point12_84723a12-3e57-4a83-b998-de086b761a36";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point12> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Point12(
            ITelegramBotClient telegramBotClient,
            ILogger<Point12> logger,
            IAnnouncementScheduler announcementScheduler)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 12;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendTextMessageAsync(
                message.Chat.Id, "Проверочная метка принята!\nВот так и должно выглядеть нормальное взаимодествие со мной.");

            return $"{ANSWER} sent";
        }
    }
}

