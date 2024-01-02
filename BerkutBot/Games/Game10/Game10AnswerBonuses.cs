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
    public class Game10AnswerBonuses : IGameAnswer
    {
        private const string REPLY_TEXT = "Бонус 1. Спеть с незнакомцами новогоднюю песню танцуя в хороводе вокруг елки (видео)." +
            "\nБонус 2. Привезти на финиш желудь." +
            "\nБонус 3. Проехать 24 метра на роликах/скейте/самокате (видео).";

        private readonly HashSet<string> _answerSet = new() {
            "Беркут, дай бонусы!",
            "Беркут дай бонусы!",
            "Беркут, дай бонусы",
            "Беркут дай бонусы",
            "Беркут, давай бонусы!",
            "Беркут давай бонусы!",
            "Беркут, давай бонусы",
            "Беркут давай бонусы",};

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Game10AnswerBonuses> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Game10AnswerBonuses(
            ITelegramBotClient telegramBotClient,
            ILogger<Game10AnswerBonuses> logger,
            IAnnouncementScheduler announcementScheduler)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent =>
            text =>
            _answerSet.Any(ans => ans.Equals(text, StringComparison.OrdinalIgnoreCase));

        public int Order => 10;

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

