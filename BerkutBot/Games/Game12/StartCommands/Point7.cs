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

namespace BerkutBot.Games.Game12.StartCommands
{
    public class Point7 : IStartCommand
    {
        private const string ANSWER = "Point7_ae3497e5-ca62-4877-84a1-4b41a02d6747";
        private const string PUBLIC_CONTAINER = "public";
        private const string BLOB_PATH = "Game12/point7.mp3";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point7> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;
        private readonly BlobServiceClient _blobServiceClient;

        public Point7(
            ITelegramBotClient telegramBotClient,
            ILogger<Point7> logger,
            IAnnouncementScheduler announcementScheduler,
            BlobServiceClient blobServiceClient)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
            _blobServiceClient = blobServiceClient;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 7;

        public async Task<string> Reply(Message message)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(PUBLIC_CONTAINER);
            var blobClient = containerClient.GetBlobClient(BLOB_PATH);
            var blobContent = await blobClient.DownloadStreamingAsync();

            var menuButtonCommands = new MenuButtonDefault();

            await _telegramBotClient.SetChatMenuButtonAsync(message.Chat.Id, menuButtonCommands);

            await _telegramBotClient.SendPhotoAsync(
                message.Chat.Id,
                InputFile.FromString("https://sawevprivate.blob.core.windows.net/public/Game12/point7.png"));

            await _telegramBotClient.SendVoiceAsync(
                message.Chat.Id,
                InputFile.FromStream(blobContent.Value.Content));

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
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game9/jokes/j6.jpg"),
                        Text = "Когда просишь у оргов 6-ю подсказку за игру"
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

