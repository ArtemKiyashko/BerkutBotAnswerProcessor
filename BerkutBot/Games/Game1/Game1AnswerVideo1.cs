using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game1
{
    public class Game1AnswerVideo1 : IGameAnswer
    {
        private const string REPLY_TEXT = "Video sent";
        private const string VIDEO_CONTAINER = "public";
        private const string VIDEO_BLOB = "BerkutVideo1_3.mp4";
        private const string ANSWER = "видео";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<Game1AnswerVideo1> _logger;

        public Game1AnswerVideo1(ITelegramBotClient telegramBotClient, BlobServiceClient blobServiceClient, ILogger<Game1AnswerVideo1> logger)
        {
            _telegramBotClient = telegramBotClient;
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public int Order => 3;

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public async Task<string> Reply(Message message)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(VIDEO_CONTAINER);
            BlobClient blob = container.GetBlobClient(VIDEO_BLOB);

            try
            {
                await _telegramBotClient.SendVideoAsync(
                    chatId: message.Chat.Id,
                    video: InputFile.FromUri(blob.Uri),
                    replyToMessageId: message.MessageId);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Cannot send video: {ex.Message}", ex);
                return $"Cannot send video: {ex.Message}";
            }
            return REPLY_TEXT;
        }
    }
}

