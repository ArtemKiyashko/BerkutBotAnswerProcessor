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
	public class Point9 : IStartCommand
	{
        private const string ANSWER = "Point9_f22b6801-fa09-4424-a2e7-960fd86091ae";

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
            await _telegramBotClient.SendPhotoAsync(message.Chat.Id, InputFile.FromUri("https://sawevprivate.blob.core.windows.net/public/Game6/point9.jpg"));
            await _telegramBotClient.SendVoiceAsync(message.Chat.Id, InputFile.FromUri("https://sawevprivate.blob.core.windows.net/public/Game6/point9.mp3"));
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
                        MessageType = MessageType.Video,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game6/jokes/smotrish_na_drugih.mp4"),
                        Text = "POV: Вы с братюнями смотрите как тупит соседняя команда"
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

