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
	public class Point5 : IStartCommand
	{
        private const string ANSWER = "Point5_a796ec50-7548-4b61-b850-735aafc447b0";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point5> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Point5(
            ITelegramBotClient telegramBotClient,
            ILogger<Point5> logger,
            IAnnouncementScheduler announcementScheduler)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 5;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendLocationAsync(message.Chat.Id, 60.021008, 29.688790);
            await SendJoke(message);
            return $"{ANSWER} coordinates sent";
        }

        private async Task SendJoke(Message message)
        {
            try
            {
                var announcement = new AnnouncementRequest()
                {
                    StartTime = DateTime.UtcNow.AddMinutes(3),
                    Chats = new List<long> { message.Chat.Id },
                    SendToAll = false,
                    Announcement = new Announcement
                    {
                        MessageType = MessageType.Photo,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game5/griffin.png")
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

