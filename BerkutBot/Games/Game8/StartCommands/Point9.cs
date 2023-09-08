using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using BerkutBot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BerkutBot.Games.Game8.StartCommands
{
	public class Point9 : IStartCommand
	{
        private const string ANSWER = "Point9_ac62c429-add3-42a2-9e20-03548312b2a7";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point9> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Point9(
            ITelegramBotClient telegramBotClient,
            ILogger<Point9> logger,
            IAnnouncementScheduler announcementScheduler)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 9;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendPhotoAsync(
                message.Chat.Id,
                InputFile.FromString("https://sawevprivate.blob.core.windows.net/public/Game8/point9.jpg"),
                caption: "24-28");
            await SendJoke(message);

            return $"{ANSWER} sent";
        }

        private async Task SendJoke(Message message)
        {
            try
            {
                var announcement = new AnnouncementRequest()
                {
                    StartTime = DateTime.UtcNow.AddMinutes(Random.Shared.Next(2, 7)),
                    Chats = new List<long> { message.Chat.Id },
                    SendToAll = false,
                    Announcement = new Announcement
                    {
                        MessageType = MessageType.Video,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game8/jokes/joke1.mp4"),
                        Text = "Когда выполнил все бонусные на игре"
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

