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
    public class Game13AnswerGo : IGameAnswer
    {
        private readonly HashSet<string> _answerSet = new() {
            "Беркут, хочу на дачу!",
            "Беркут, хочу на дачу",
            "Беркут хочу на дачу!",
            "Беркут хочу на дачу"};


        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Game13AnswerGo> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Game13AnswerGo(ITelegramBotClient telegramBotClient,
            ILogger<Game13AnswerGo> logger,
            IAnnouncementScheduler announcementScheduler)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
        }

        public Func<string, bool> Intent =>
            text =>
            _answerSet.Any(ans => ans.Equals(text, StringComparison.OrdinalIgnoreCase));

        public int Order => 0;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendPhotoAsync(
                chatId: message.Chat.Id,
                photo: InputFile.FromString("https://sawevprivate.blob.core.windows.net/public/Game13/point0.png"));

            await SendBonuses(message);
            await ScheduleBonus(message);

            return $"Photo sent";
        }

        private async Task SendBonuses(Message message)
        {
            await _telegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Бонус 1:\nДомашнее задание.");

            await _telegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Бонус 2:\nПривезти на финиш каштаны.");

            await _telegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Бонус 3:\nКоманде необходимо всем составом сфотографироваться с Volkswagen Beetle. Не моделька, не картинка - ничего подобного. Настоящий, полноразмерный автомобиль на дороге.");

            await _telegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Бонус 4:\nСпеть фрагмент песни Игоря Скляра \"Комарово\" с двумя дорожными рабочими всем составом. \nСнять на видео, прислать оргам.");
        }

        private async Task ScheduleBonus(Message message)
        {
            var todayEvening = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 18, 55, 00, DateTimeKind.Utc);
            try
            {
                var announcement = new AnnouncementRequest()
                {
                    StartTime = todayEvening,
                    Chats = new List<long> { message.Chat.Id },
                    SendToAll = false,
                    Announcement = new Announcement
                    {
                        MessageType = MessageType.Voice,
                        ContentUrl = new Uri("https://sawevprivate.blob.core.windows.net/public/Game13/bonus.mp3")
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

