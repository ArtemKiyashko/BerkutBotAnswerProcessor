using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using BerkutBot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BerkutBot.Games.Game7.StartCommands
{
	public class Point6 : IStartCommand
	{
        private const string ANSWER = "Point6_2186e286-c1a6-4173-be49-be9de73ea325";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point6> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Point6(
            ITelegramBotClient telegramBotClient,
            ILogger<Point6> logger,
            IAnnouncementScheduler announcementScheduler)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 6;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendPhotoAsync(message.Chat.Id, InputFile.FromUri("https://sawevprivate.blob.core.windows.net/public/Game7/point_6.jpg"));
            //await SendJoke(message);
            return $"{ANSWER} sent";
        }

        private async Task SendJoke(Message message)
        {
            try
            {
                var announcement = new AnnouncementRequest()
                {
                    StartTime = DateTime.UtcNow.AddMinutes(5),
                    Chats = new List<long> { message.Chat.Id },
                    SendToAll = false,
                    Announcement = new Announcement
                    {
                        MessageType = MessageType.Photo,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game6/jokes/smuhlevali.jpg"),
                        Text = "Когда смухлевал и это спалили орги"
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

