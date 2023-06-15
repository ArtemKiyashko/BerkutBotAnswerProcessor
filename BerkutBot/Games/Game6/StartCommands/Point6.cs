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
	public class Point6 : IStartCommand
	{
        private const string ANSWER = "Point6_21fdb832-94d2-4521-b4b3-eeca00cad129";

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
            /*var containerClient = _blobServiceClient.GetBlobContainerClient("public");
            var blobClient = containerClient.GetBlobClient("Game6/point6_1.JPG");
            var blobContent = await blobClient.DownloadStreamingAsync();*/

            await _telegramBotClient.SendDocumentAsync(message.Chat.Id, InputFile.FromUri("https://sawevprivate.blob.core.windows.net/public/Game6/berkut_game6_pic_1.jpg"));
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
                        MessageType = MessageType.Photo,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game6/jokes/smuhlevali.jpg"),
                        Text = "Когда смухлевал и это спалили орги"
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

