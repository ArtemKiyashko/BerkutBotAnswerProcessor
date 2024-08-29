using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using BerkutBot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BerkutBot.Games.Game13
{
    public class Game13AnswerGora : IGameAnswer
    {
        private readonly HashSet<string> _answerSet = new() {
            "Змей Горыныч"};

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Game13AnswerGora> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Game13AnswerGora(ITelegramBotClient telegramBotClient,
            ILogger<Game13AnswerGora> logger,
            IAnnouncementScheduler announcementScheduler)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent =>
            text =>
            _answerSet.Any(ans => ans.Equals(text, StringComparison.OrdinalIgnoreCase));

        public int Order => 5;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendVideoAsync(
                chatId: message.Chat.Id,
                video: InputFile.FromString("https://sawevprivate.blob.core.windows.net/public/Game13/point5_0.mp4"));

            await SendAdditionalVideo(message);

            return $"Video sent";
        }

        private async Task SendAdditionalVideo(Message message)
        {
            try
            {
                var announcement = new AnnouncementRequest()
                {
                    StartTime = DateTime.UtcNow.AddSeconds(20),
                    Chats = new List<long> { message.Chat.Id },
                    SendToAll = false,
                    Announcement = new Announcement
                    {
                        MessageType = MessageType.Video,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game13/point5.mp4")
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

