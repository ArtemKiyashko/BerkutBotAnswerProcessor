using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using BerkutBot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BerkutBot.Games.Game12.StartCommands
{
	public class Point1 : IStartCommand
	{
        private const string ANSWER = "Point1_1ea681af-258d-47d0-b085-b787d1dd56a4";

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
            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                InlineKeyboardButton.WithUrl(
                    text: "iOS 3D",
                    url: "https://berkut.ar/models/point1_ios.usdz"),
                InlineKeyboardButton.WithUrl(
                    text: "Android 3D",
                    url: "https://berkut.ar/models/point1_android.glb")
            });
            await _telegramBotClient.SendVideoAsync(
                chatId: message.Chat.Id,
                video: InputFile.FromString("https://sawevprivate.blob.core.windows.net/public/Game12/point1.mp4"),
                replyMarkup: inlineKeyboard);

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

