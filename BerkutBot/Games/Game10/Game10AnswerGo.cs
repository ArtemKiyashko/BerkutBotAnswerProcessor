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

namespace BerkutBot.Games.Game10
{
    public class Game10AnswerGo : IGameAnswer
    {
        private const string REPLY_TEXT = "🌊   🇩🇪 🇦🇹 🇸🇰 🇭🇺 🇭🇷 🇷🇸 🇧🇬 🇷🇴 🇺🇦 🇲🇩 ski\nД. 7, К. 7.";

        private readonly HashSet<string> _answerSet = new() {
            "Беркут, холодно!",
            "Беркут холодно!",
            "Беркут, холодно",
            "Беркут холодно",};
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Game10AnswerGo> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Game10AnswerGo(
            ITelegramBotClient telegramBotClient,
            ILogger<Game10AnswerGo> logger,
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
            await _telegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: REPLY_TEXT);

            //await SendJoke(message);

            return REPLY_TEXT;
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
                        MessageType = MessageType.Video,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game5/rebus.mp4")
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

