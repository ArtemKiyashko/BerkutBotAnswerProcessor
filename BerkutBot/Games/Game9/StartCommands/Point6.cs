﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using BerkutBot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BerkutBot.Games.Game9.StartCommands
{
	public class Point6 : IStartCommand
	{
        private const string ANSWER = "Point6_72815600-b634-493d-8a50-ea2de87e7990";

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
            await _telegramBotClient.SendPhotoAsync(
                message.Chat.Id,
                InputFile.FromString("https://sawevprivate.blob.core.windows.net/public/Game9/q6.jpg"));

            await SendJoke(message);

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

