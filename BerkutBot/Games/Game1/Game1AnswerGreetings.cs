using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BerkutBot.Games.Game1.Options;
using BerkutBot.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game1
{
    public class Game1AnswerGreetings : IGame1Answer
    {
        private const string REPLY_TEXT = "Video sent";
        private const string VIDEO_CONTAINER = "public";
        private const string VIDEO_BLOB = "BerkutGreetings.mp4";
        private const string ANSWER = "start";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<Game1AnswerGreetings> _logger;

        public Game1AnswerGreetings(ITelegramBotClient telegramBotClient, BlobServiceClient blobServiceClient, ILogger<Game1AnswerGreetings> logger)
        {
            _telegramBotClient = telegramBotClient;
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public Func<string, bool> Intent => FormatMessage;

        public int Order => 2;

        private bool FormatMessage(string text)
            => !string.IsNullOrEmpty(text) && text.Replace("/", "").ToLower().Equals(ANSWER, StringComparison.OrdinalIgnoreCase);

        public async Task<string> Reply(Message message)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(VIDEO_CONTAINER);
            BlobClient blob = container.GetBlobClient(VIDEO_BLOB);

            try
            {
                await _telegramBotClient.SendVideoAsync(
                    chatId: message.Chat.Id,
                    video: blob.Uri.AbsoluteUri);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot send video: {ex.Message}", ex);
                return $"Cannot send video: {ex.Message}";
            }
            return REPLY_TEXT;
        }
    }
}

