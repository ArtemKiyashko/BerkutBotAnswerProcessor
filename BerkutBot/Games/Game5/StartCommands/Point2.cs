using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using BerkutBot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BerkutBot.Games.Game5.StartCommands
{
	public class Point2 : IStartCommand
	{
        private const string ANSWER = "Point2_d0bdfb55-4c60-4323-816c-441ce7bb791a";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point2> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Point2(
            ITelegramBotClient telegramBotClient,
            ILogger<Point2> logger,
            IAnnouncementScheduler announcementScheduler)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 2;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendLocationAsync(message.Chat.Id, 59.87952, 29.86228);
            await SendJoke(message);

            return $"{ANSWER} coordinates sent";
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
                        MessageType = MessageType.Photo,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game5/next_point.jpg")
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

