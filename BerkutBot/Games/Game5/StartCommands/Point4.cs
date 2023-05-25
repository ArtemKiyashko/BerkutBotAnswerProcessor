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

namespace BerkutBot.Games.Game5.StartCommands
{
	public class Point4 : IStartCommand
	{
        private const string ANSWER = "Point4_ed6b2cac-bfd4-4b06-a210-82bc6fe1bebf";
        private const string PUBLIC_CONTAINER = "public";
        private const string BLOB_PATH = "Game5/point4_sound.mp3";
        private const string ANSWER_TEXT = "https://yandex.ru/maps/-/CCUsMKUOhC";
        private const string ANSWER_PIC_URL = "https://sawevprivate.blob.core.windows.net/public/Game5/point4_pic.jpeg";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point4> _logger;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Point4(
            ITelegramBotClient telegramBotClient,
            ILogger<Point4> logger,
            BlobServiceClient blobServiceClient,
            IAnnouncementScheduler announcementScheduler)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _blobServiceClient = blobServiceClient;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 4;

        public async Task<string> Reply(Message message)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(PUBLIC_CONTAINER);
            var blobClient = containerClient.GetBlobClient(BLOB_PATH);
            var blobContent = await blobClient.DownloadStreamingAsync();

            await _telegramBotClient.SendVoiceAsync(message.Chat.Id, InputFile.FromStream(blobContent.Value.Content));

            await _telegramBotClient.SendPhotoAsync(message.Chat.Id, InputFile.FromUri(ANSWER_PIC_URL), caption: ANSWER_TEXT);

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
                        MessageType = MessageType.Photo,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game5/nunu.jpeg")
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

