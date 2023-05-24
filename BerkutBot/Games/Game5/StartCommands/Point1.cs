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
	public class Point1 : IStartCommand
	{
        private const string ANSWER = "Point1_7aaa6cfc-bc2e-4b2c-9b59-88c62d52072f";

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
            await _telegramBotClient.SendLocationAsync(message.Chat.Id, 59.84886, 30.00002);
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
                        MessageType = MessageType.Sticker,
                        Text = "CAACAgIAAxkBAAIeGWRuVywT43OOlecw7lV9T7fSa1s1AAI_IAACmVqgSYFs2ieXWVKaLwQ"
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

