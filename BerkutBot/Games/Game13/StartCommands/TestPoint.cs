using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using BerkutBot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BerkutBot.Games.Game13.StartCommands
{
	public class TestPoint : IStartCommand
	{
        private const string ANSWER = "TestPoint_84723a12-3e57-4a83-b998-de086b761a36";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IAnnouncementScheduler _announcementScheduler;
        private readonly ILogger<TestPoint> _logger;

        public TestPoint(
            ITelegramBotClient telegramBotClient,
            IAnnouncementScheduler announcementScheduler,
            ILogger<TestPoint> logger)
		{
            _telegramBotClient = telegramBotClient;
            _announcementScheduler = announcementScheduler;
            _logger = logger;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 99;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendTextMessageAsync(
                message.Chat.Id, "Проверочная метка принята!\nВот так и должно выглядеть нормальное взаимодествие со мной.");

            await ScheduleMemes(message);

            return $"{ANSWER} sent";
        }


        private async Task ScheduleMemes(Message message)
        {
            var todayEvening = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 17, 30, 00, DateTimeKind.Utc);
            try
            {
                var announcement = CreateAnnouncement(message, todayEvening, MessageType.Photo, "https://sawevprivate.blob.core.windows.net/public/Game13/memes/meme1.jpg");
                await _announcementScheduler.ScheduleAnnouncement(announcement);

                announcement = CreateAnnouncement(message, todayEvening.AddMinutes(30), MessageType.Video, "https://sawevprivate.blob.core.windows.net/public/Game13/memes/meme2.mp4");
                await _announcementScheduler.ScheduleAnnouncement(announcement);

                announcement = CreateAnnouncement(message, todayEvening.AddMinutes(60), MessageType.Photo, "https://sawevprivate.blob.core.windows.net/public/Game13/memes/meme3.jpg");
                await _announcementScheduler.ScheduleAnnouncement(announcement);

                announcement = CreateAnnouncement(message, todayEvening.AddMinutes(90), MessageType.Video, "https://sawevprivate.blob.core.windows.net/public/Game13/memes/meme4.mp4");
                await _announcementScheduler.ScheduleAnnouncement(announcement);

                announcement = CreateAnnouncement(message, todayEvening.AddMinutes(120), MessageType.Photo, "https://sawevprivate.blob.core.windows.net/public/Game13/memes/meme5.jpg");
                await _announcementScheduler.ScheduleAnnouncement(announcement);

                announcement = CreateAnnouncement(message, todayEvening.AddMinutes(150), MessageType.Video, "https://sawevprivate.blob.core.windows.net/public/Game13/memes/meme6.mp4");
                await _announcementScheduler.ScheduleAnnouncement(announcement);

                announcement = CreateAnnouncement(message, todayEvening.AddMinutes(180), MessageType.Photo, "https://sawevprivate.blob.core.windows.net/public/Game13/memes/meme7.jpg");
                await _announcementScheduler.ScheduleAnnouncement(announcement);

                announcement = CreateAnnouncement(message, todayEvening.AddMinutes(210), MessageType.Photo, "https://sawevprivate.blob.core.windows.net/public/Game13/memes/meme8.jpg");
                await _announcementScheduler.ScheduleAnnouncement(announcement);

                announcement = CreateAnnouncement(message, todayEvening.AddMinutes(240), MessageType.Photo, "https://sawevprivate.blob.core.windows.net/public/Game13/memes/meme9.jpg");
                await _announcementScheduler.ScheduleAnnouncement(announcement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send an announcement");
            }
        }

        private static AnnouncementRequest CreateAnnouncement(Message message, DateTime todayEvening, MessageType messageType, string contentUrl)
        => new()
        {
            StartTime = todayEvening,
            Chats = new List<long> { message.Chat.Id },
            SendToAll = false,
            Announcement = new Announcement
            {
                MessageType = messageType,
                ContentUrl = new Uri(contentUrl)
            }
        };

    }
}

