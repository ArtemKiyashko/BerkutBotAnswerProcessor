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

namespace BerkutBot.Games.Game8
{
    public class Game8AnswerGo : IGameAnswer
    {
        private const string REPLY_TEXT = "Park in ✌️ phone tan";

        private readonly HashSet<string> _answerSet = new() { "Беркут гони", "Беркут, гони!", "Беркут, гони", "Беркут погнали", "Буркут, погнали!", "Беркут, погнали" };
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Game8AnswerGo> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Game8AnswerGo(
            ITelegramBotClient telegramBotClient,
            ILogger<Game8AnswerGo> logger,
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
                text: REPLY_TEXT,
                replyToMessageId: message.MessageId);

            //await SendJoke(message);

            return REPLY_TEXT;
        }

        private async Task SendJoke(Message message)
        {
            try
            {
                var announcement = new AnnouncementRequest()
                {
                    StartTime = DateTime.UtcNow.AddMinutes(3),
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

