﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BerkutBot.Infrastructure;
using BerkutBot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BerkutBot.Games.Game13.StartCommands
{
	public class Point5 : IStartCommand
	{
        private const string ANSWER = "Point5_df6c8bee-41ce-4cc9-b032-c153b2cc6dbc";
        private const string PUBLIC_CONTAINER = "public";
        private const string BLOB_PATH = "Game13/point5_init_converted.mp4";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point5> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;
        private readonly BlobServiceClient _blobServiceClient;

        public Point5(
            ITelegramBotClient telegramBotClient,
            ILogger<Point5> logger,
            IAnnouncementScheduler announcementScheduler,
            BlobServiceClient blobServiceClient)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
            _blobServiceClient = blobServiceClient;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 5;

        public async Task<string> Reply(Message message)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(PUBLIC_CONTAINER);
            var blobClient = containerClient.GetBlobClient(BLOB_PATH);
            var blobContent = await blobClient.DownloadStreamingAsync();

            await _telegramBotClient.SendVideoAsync(
                chatId: message.Chat.Id,
                video: InputFile.FromStream(blobContent.Value.Content),
                width: 720,
                height: 1280,
                supportsStreaming: true);

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

