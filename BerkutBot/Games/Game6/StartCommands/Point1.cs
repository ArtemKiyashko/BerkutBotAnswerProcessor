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
	public class Point1 : IStartCommand
	{
        private const string ANSWER = "Point1_71fda70f-58f1-4c82-b3d9-38d1750f3e47";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point1> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;
        private readonly BlobServiceClient _blobServiceClient;

        public Point1(
            ITelegramBotClient telegramBotClient,
            ILogger<Point1> logger,
            IAnnouncementScheduler announcementScheduler,
            BlobServiceClient blobServiceClient)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
            _blobServiceClient = blobServiceClient;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 1;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendPhotoAsync(message.Chat.Id, InputFile.FromUri("https://sawevprivate.blob.core.windows.net/public/Game6/point1.png"));
            await _telegramBotClient.SendVoiceAsync(message.Chat.Id, InputFile.FromUri("https://sawevprivate.blob.core.windows.net/public/Game6/point1.mp3"));

            await SendJoke(message);

            return $"{ANSWER} sent";
        }

        private async Task SendJoke(Message message)
        {
            try
            {
                var announcement1 = new AnnouncementRequest()
                {
                    StartTime = DateTime.UtcNow.AddMinutes(3),
                    Chats = new List<long> { message.Chat.Id },
                    SendToAll = false,
                    Announcement = new Announcement
                    {
                        MessageType = MessageType.Voice,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game6/jokes/edesh_ne_tuda.mp3")
                    }
                };
                await _announcementScheduler.ScheduleAnnouncement(announcement1);

                var announcement2 = new AnnouncementRequest()
                {
                    StartTime = DateTime.UtcNow.AddMinutes(6),
                    Chats = new List<long> { message.Chat.Id },
                    SendToAll = false,
                    Announcement = new Announcement
                    {
                        MessageType = MessageType.Voice,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game6/jokes/its_a_joke.mp3")
                    }
                };
                await _announcementScheduler.ScheduleAnnouncement(announcement2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send an announcement");
            }
        }
    }
}

