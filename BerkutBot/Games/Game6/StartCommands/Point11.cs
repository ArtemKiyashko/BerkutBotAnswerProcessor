using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BerkutBot.Infrastructure;
using BerkutBot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BerkutBot.Games.Game6.StartCommands
{
	public class Point11 : IStartCommand
	{
        private const string ANSWER = "Point11_9aeef210-bbf6-467e-b3ce-d99dd1e20a0e";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point11> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Point11(
            ITelegramBotClient telegramBotClient,
            ILogger<Point11> logger,
            IAnnouncementScheduler announcementScheduler)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 11;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendTextMessageAsync(
                message.Chat.Id,
                "https://rb.gy/tple2\nhttps://rb.gy/m8gsb\nhttps://rb.gy/42n25\nhttps://rb.gy/l4uhi\n[6romv/yg.br//:sptth](https://rb.gy/vmor6)\nhttps://rb.gy/i28uw\nhttps://rb.gy/toyfl\n\nhttps://rb.gy/35nia\nhttps://rb.gy/fcbmf\nhttps://rb.gy/7pjxd",
                disableWebPagePreview: true,
                parseMode: ParseMode.Markdown);
            await SendJoke(message);
            return $"{ANSWER} sent";
        }

        private async Task SendJoke(Message message)
        {
            try
            {
                var announcement = new AnnouncementRequest()
                {
                    StartTime = DateTime.UtcNow.AddMinutes(2),
                    Chats = new List<long> { message.Chat.Id },
                    SendToAll = false,
                    Announcement = new Announcement
                    {
                        MessageType = MessageType.Video,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game6/jokes/sezdil_na_kvest.mp4"),
                        Text = "Съездил на этот ваш автаквест..."
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

