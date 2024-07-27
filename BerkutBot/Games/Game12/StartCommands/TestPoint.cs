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
	public class TestPoint : IStartCommand
	{
        private const string ANSWER = "TestPoint_84723a12-3e57-4a83-b998-de086b761a36";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IAnnouncementScheduler _announcementScheduler;
        private readonly ILogger<TestPoint> _logger;

        public TestPoint(
            ITelegramBotClient telegramBotClient,
            IAnnouncementScheduler announcementScheduler,
            ILogger<TestPoint> logger)
		{
            _telegramBotClient = telegramBotClient;
            _announcementScheduler = announcementScheduler;
            _logger = logger;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 99;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendTextMessageAsync(
                message.Chat.Id, "Проверочная метка принята!\nВот так и должно выглядеть нормальное взаимодествие со мной.");

            return $"{ANSWER} sent";
        }

        private async Task SendJoke(DateTime startDateTime, long chatId, MessageType messageType, string contentUrl, string text = null)
        {
            try
            {
                var announcementRequest = CreateAnnouncementRequest(startDateTime, chatId, messageType, contentUrl);
                await _announcementScheduler.ScheduleAnnouncement(announcementRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send an announcement");
            }
        }

        private static AnnouncementRequest CreateAnnouncementRequest(DateTime startDateTime, long chatId, MessageType messageType, string contentUrl, string text = null)
        =>
            new()
            {
                StartTime = startDateTime,
                Chats = new List<long> { chatId },
                SendToAll = false,
                Announcement = new Announcement
                {
                    MessageType = messageType,
                    ContentUrl = new Uri(contentUrl),
                    Text = text
                }
            };
        
    }
}

