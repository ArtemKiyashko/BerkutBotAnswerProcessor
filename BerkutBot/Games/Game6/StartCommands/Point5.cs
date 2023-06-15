using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using BerkutBot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BerkutBot.Games.Game6.StartCommands
{
	public class Point5 : IStartCommand
	{
        private const string ANSWER = "Point5_7d35d86f-36c7-4bc2-8e05-b3c571463c37";

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
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "https://spb.hh.ru/employer/2220251", disableWebPagePreview: true);
            await SendJoke(message);
            return $"{ANSWER} sent";
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
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game6/jokes/nashli_metku.jpg"),
                        Text = "Когда наконец нашли метку"
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

