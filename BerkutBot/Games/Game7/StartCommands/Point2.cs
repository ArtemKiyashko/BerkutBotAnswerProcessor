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
	public class Point2 : IStartCommand
	{
        private const string ANSWER = "Point2_62ba42fa-c4e6-474d-9a09-d2dcfc3ad369";

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
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "[ССЫЛКА](https://gazgolder.com/)\n3АЛИТ3", disableWebPagePreview: true, parseMode: ParseMode.Markdown);
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
                        MessageType = MessageType.Video,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game6/jokes/na_brifinge_result.mp4")
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

