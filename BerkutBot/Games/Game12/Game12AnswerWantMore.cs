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

namespace BerkutBot.Games.Game12
{
    public class Game12AnswerWantMore : IGameAnswer
    {
        private const string REPLY_TEXT = "326 м";

        private readonly HashSet<string> _answerSet = new() {
            "Беркут, хочу еще!",
            "Беркут, хочу еще",
            "Беркут хочу еще!",
            "Беркут хочу еще",
            "Беркут, хочу ещё!",
            "Беркут, хочу ещё",
            "Беркут хочу ещё!",
            "Беркут хочу ещё",};


        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Game12AnswerWantMore> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Game12AnswerWantMore(ITelegramBotClient telegramBotClient,
            ILogger<Game12AnswerWantMore> logger,
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
            await _telegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: REPLY_TEXT);

            //await SendJoke(message);

            return $"{REPLY_TEXT} sent";
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

