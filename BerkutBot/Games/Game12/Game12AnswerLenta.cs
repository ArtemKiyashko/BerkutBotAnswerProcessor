using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BerkutBot.Infrastructure;
using BerkutBot.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BerkutBot.Games.Game12
{
    public class Game12AnswerLenta : IGameAnswer
    {
        private const string PUBLIC_CONTAINER = "public";
        private const string BLOB_PATH = "Game12/point10.mp3";

        private readonly HashSet<string> _answerSet = new() {
            "космос прибалтийская"};


        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Game12AnswerLenta> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;
        private readonly BlobServiceClient _blobServiceClient;

        public Game12AnswerLenta(ITelegramBotClient telegramBotClient,
            ILogger<Game12AnswerLenta> logger,
            IAnnouncementScheduler announcementScheduler,
            BlobServiceClient blobServiceClient)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _announcementScheduler = announcementScheduler;
            _blobServiceClient = blobServiceClient;
        }

        public Func<string, bool> Intent =>
            text =>
            _answerSet.Any(ans => ans.Equals(text, StringComparison.OrdinalIgnoreCase));

        public int Order => 13;

        public async Task<string> Reply(Message message)
        {
            var menuButtonCommands = new MenuButtonDefault();

            await _telegramBotClient.SetChatMenuButtonAsync(message.Chat.Id, menuButtonCommands);

            await _telegramBotClient.SendLocationAsync(
                chatId: message.Chat.Id,
                latitude: 59.938899,
                longitude: 30.214655);

            //await SendJoke(message);

            return $"Location sent";
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

