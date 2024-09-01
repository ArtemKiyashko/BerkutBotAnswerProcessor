﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using BerkutBot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BerkutBot.Games.Game13.StartCommands
{
	public class Point1 : IStartCommand
	{
        private const string ANSWER = "Point1_4be124f6-ec0c-498d-b20e-c5b22f79b700";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point1> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Point1(
            ITelegramBotClient telegramBotClient,
            ILogger<Point1> logger,
            IAnnouncementScheduler announcementScheduler)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 1;

        public async Task<string> Reply(Message message)
        {
            
            await _telegramBotClient.SendPhotoAsync(
                chatId: message.Chat.Id,
                photo: InputFile.FromString("https://sawevprivate.blob.core.windows.net/public/Game13/point1.jpg"));

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
