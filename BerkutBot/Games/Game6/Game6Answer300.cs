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

namespace BerkutBot.Games.Game6
{
    public class Game6Answer300 : IGameAnswer
    {

        private readonly HashSet<string> _answerSet = new() { "300", "триста" };
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Game6Answer300> _logger;
        private readonly IAnnouncementScheduler _announcementScheduler;

        public Game6Answer300(
            ITelegramBotClient telegramBotClient,
            ILogger<Game6Answer300> logger,
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
            await _telegramBotClient.SendPhotoAsync(message.Chat.Id, InputFile.FromUri("https://sawevprivate.blob.core.windows.net/public/Game6/jokes/300.png"));

            return $"300 answer sent";
        }
    }
}

