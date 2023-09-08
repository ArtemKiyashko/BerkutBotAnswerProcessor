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
	public class Point7 : IStartCommand
	{
        private const string ANSWER = "Point7_fe023abc-30e4-43b7-9196-1b4cec1279d6";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point7> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Point7(
            ITelegramBotClient telegramBotClient,
            ILogger<Point7> logger,
            IAnnouncementScheduler announcementScheduler)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 7;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendPhotoAsync(
                message.Chat.Id,
                InputFile.FromString("https://sawevprivate.blob.core.windows.net/public/Game8/point7.png"),
                caption: "79  37(b=o)  88");
            //await SendJoke(message);

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
                        MessageType = MessageType.Photo,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game7/jokes/zakladka.jpg"),
                        Text = "Орги раскидывают метки"
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

