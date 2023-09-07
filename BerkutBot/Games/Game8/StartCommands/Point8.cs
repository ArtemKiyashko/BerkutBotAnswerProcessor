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
	public class Point8 : IStartCommand
	{
        private const string ANSWER = "Point8_889e7182-897f-44c2-9230-c2c9be71aa87";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point8> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Point8(
            ITelegramBotClient telegramBotClient,
            ILogger<Point8> logger,
            IAnnouncementScheduler announcementScheduler)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 8;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendTextMessageAsync(
                message.Chat.Id,
                "https://souz.parts/#contacts");
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

