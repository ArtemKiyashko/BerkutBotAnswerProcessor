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

namespace BerkutBot.Games.Game5.StartCommands
{
	public class Point6 : IStartCommand
	{
        private const string ANSWER = "Point6_b9913729-3ef7-4760-9f48-fc4e745f551f";
        private const string ANSWER_VIDEO_URL = "https://youtu.be/17AyzxWNHBM";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point6> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Point6(
            ITelegramBotClient telegramBotClient,
            ILogger<Point6> logger,
            IAnnouncementScheduler announcementScheduler)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 6;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, ANSWER_VIDEO_URL, disableWebPagePreview: true);
            await SendJoke(message);
            return $"{ANSWER} coordinates sent";
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
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game5/BerkutGame5_joke6.mp4")
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

