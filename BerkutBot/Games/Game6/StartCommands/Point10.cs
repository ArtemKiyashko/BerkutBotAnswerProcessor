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
	public class Point10 : IStartCommand
	{
        private const string ANSWER = "Point10_b104dbbf-b6c5-4137-89a2-80b7a6e985c3";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point10> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Point10(
            ITelegramBotClient telegramBotClient,
            ILogger<Point10> logger,
            IAnnouncementScheduler announcementScheduler)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 10;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "https://www.gov.spb.ru/gov/otrasl/c_govcontrol/news/29285/", disableWebPagePreview: true);
            await SendJoke(message);
            return $"{ANSWER} sent";
        }

        private async Task SendJoke(Message message)
        {
            try
            {
                var announcement = new AnnouncementRequest()
                {
                    StartTime = DateTime.UtcNow.AddMinutes(2),
                    Chats = new List<long> { message.Chat.Id },
                    SendToAll = false,
                    Announcement = new Announcement
                    {
                        MessageType = MessageType.Text,
                        Text = "Скажи 300"
                    }
                };
                await _announcementScheduler.ScheduleAnnouncement(announcement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send an announcement");
            }
        }
    }
}

