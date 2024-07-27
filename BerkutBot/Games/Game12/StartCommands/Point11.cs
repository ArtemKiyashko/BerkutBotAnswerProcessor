using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using BerkutBot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BerkutBot.Games.Game12.StartCommands
{
	public class Point11 : IStartCommand
	{
        private const string ANSWER = "Point11_bc6bf456-83a3-4366-abef-5c39abc14268";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Point11> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Point11(
            ITelegramBotClient telegramBotClient,
            ILogger<Point11> logger,
            IAnnouncementScheduler announcementScheduler)
		{
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 11;

        public async Task<string> Reply(Message message)
        {
            var menuButtonCommands = new MenuButtonDefault();

            await _telegramBotClient.SetChatMenuButtonAsync(message.Chat.Id, menuButtonCommands);

            await _telegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "111011\n11101011100100100110\n11110\n111011100001011101");

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

