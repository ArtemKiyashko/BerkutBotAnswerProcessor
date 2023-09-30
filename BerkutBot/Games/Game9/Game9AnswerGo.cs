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

namespace BerkutBot.Games.Game9
{
    public class Game9AnswerGo : IGameAnswer
    {
        private const string REPLY_TEXT = "Game9 begin";

        private readonly HashSet<string> _answerSet = new() {
            "Winter2024quest",};
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Game9AnswerGo> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Game9AnswerGo(
            ITelegramBotClient telegramBotClient,
            ILogger<Game9AnswerGo> logger,
            IAnnouncementScheduler announcementScheduler)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent =>
            text =>
            _answerSet.Any(ans => ans.Equals(text, StringComparison.OrdinalIgnoreCase));

        public int Order => 1;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendVideoAsync(
                chatId: message.Chat.Id,
                InputFile.FromString("https://sawevprivate.blob.core.windows.net/public/Game9/game9_q1_1.mp4"));

            await SendNextPart(message, 2, "https://sawevprivate.blob.core.windows.net/public/Game9/game9_q1_2.mp4");
            await SendNextPart(message, 4, "https://sawevprivate.blob.core.windows.net/public/Game9/game9_q1_3.mp4");

            return REPLY_TEXT;
        }

        private async Task SendNextPart(Message message, int delayMinutes, string videoUrl)
        {
            try
            {
                var announcement = new AnnouncementRequest()
                {
                    StartTime = DateTime.UtcNow.AddMinutes(delayMinutes),
                    Chats = new List<long> { message.Chat.Id },
                    SendToAll = false,
                    Announcement = new Announcement
                    {
                        MessageType = MessageType.Video,
                        ContentUrl = new Uri(videoUrl)
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

