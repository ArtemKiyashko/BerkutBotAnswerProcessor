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
    public class Game13AnswerWhiteRose : IGameAnswer
    {
        private readonly HashSet<string> _answerSet = new() {
            "Белые розы.",
            "Белые розы"};

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Game13AnswerWhiteRose> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Game13AnswerWhiteRose(ITelegramBotClient telegramBotClient,
            ILogger<Game13AnswerWhiteRose> logger,
            IAnnouncementScheduler announcementScheduler)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent =>
            text =>
            _answerSet.Any(ans => ans.Equals(text, StringComparison.OrdinalIgnoreCase));

        public int Order => 4;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendPhotoAsync(
                chatId: message.Chat.Id,
                photo: InputFile.FromString("https://sawevprivate.blob.core.windows.net/public/Game13/point4.png"));

            //await SendJoke(message);

            return $"Photo sent";
        }

        private async Task SendJoke(Message message)
        {
            try
            {
                var announcement = new AnnouncementRequest()
                {
                    StartTime = DateTime.UtcNow.AddMinutes(1),
                    Chats = new List<long> { message.Chat.Id },
                    SendToAll = false,
                    Announcement = new Announcement
                    {
                        MessageType = MessageType.Text,
                        Text = "Бонусные  задания:\n\n" +
                        "1. Привести монету номиналом 1 копейка.\n" +
                        "2. Сфотографироваться всем экипажем с аэроменом.\n" +
                        "3. Пробежать с незнакомцем \"королевскую\" дистанцию на время. Снять на видео. \n" +
                        "4. Спеть припев песни \"Ты морячка я моряк\" с военным моряком. Снять на видео.\n\n" +
                        "Каждый выполненный бонус - 10 минут."
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

