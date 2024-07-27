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
    public class Point6 : IStartCommand
    {
        private const string ANSWER = "Point6_f9ef7318-4367-4596-bd3f-9db681973898";
        private const string PUBLIC_CONTAINER = "public";
        private const string BLOB_PATH = "Game12/point6.mp3";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point6> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;
        private readonly BlobServiceClient _blobServiceClient;

        public Point6(
            ITelegramBotClient telegramBotClient,
            ILogger<Point6> logger,
            IAnnouncementScheduler announcementScheduler,
            BlobServiceClient blobServiceClient)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
            _blobServiceClient = blobServiceClient;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 6;

        public async Task<string> Reply(Message message)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(PUBLIC_CONTAINER);
            var blobClient = containerClient.GetBlobClient(BLOB_PATH);
            var blobContent = await blobClient.DownloadStreamingAsync();

            var menuButtonLamp = new MenuButtonWebApp
            {
                WebApp = new WebAppInfo { Url = "https://berkut.ar/lampservice/" },
                Text = "💡"
            };
            await _telegramBotClient.SetChatMenuButtonAsync(message.Chat.Id, menuButtonLamp);

            await _telegramBotClient.SendVoiceAsync(
                message.Chat.Id,
                InputFile.FromStream(blobContent.Value.Content));

            await SendJoke(message);

            return $"{ANSWER} sent";
        }

        private async Task SendJoke(Message message)
        {
            try
            {
                var announcement = new AnnouncementRequest()
                {
                    StartTime = DateTime.UtcNow.AddSeconds(60),
                    Chats = new List<long> { message.Chat.Id },
                    SendToAll = false,
                    Announcement = new Announcement
                    {
                        MessageType = MessageType.Voice,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game12/point6_2.mp3")
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

