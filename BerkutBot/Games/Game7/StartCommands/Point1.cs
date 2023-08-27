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
	public class Point1 : IStartCommand
	{
        private const string ANSWER = "Point1_88ef1f52-ee87-4fa8-9f6d-e3886c17240d";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point1> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Point1(
            ITelegramBotClient telegramBotClient,
            ILogger<Point1> logger,
            IAnnouncementScheduler announcementScheduler)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 1;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendLocationAsync(message.Chat.Id, 59.882645, 30.300472);

            await SendJoke(message);

            return $"{ANSWER} sent";
        }

        private async Task SendJoke(Message message)
        {
            try
            {
                var announcement1 = new AnnouncementRequest()
                {
                    StartTime = DateTime.UtcNow.AddMinutes(10),
                    Chats = new List<long> { message.Chat.Id },
                    SendToAll = false,
                    Announcement = new Announcement
                    {
                        MessageType = MessageType.Photo,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game7/jokes/game_tutorials_edit.jpg"),
                        Text = "Когда пропустил брифинг и поехал на квест"
                    }
                };
                await _announcementScheduler.ScheduleAnnouncement(announcement1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send an announcement");
            }
        }
    }
}

