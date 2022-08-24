using System;
using Azure.Storage.Blobs;
using BerkutBot.Games.Game1;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Telegram.Bot;
using BerkutBot.Infrastructure;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game2
{
	public class Game2AnswerGreetings : IGameAnswer
	{
        private const string REPLY_TEXT = "Picture sent";
        private const string VIDEO_CONTAINER = "public";
        private const string VIDEO_BLOB = "HogwartsExpress.jpg";
        private const string ANSWER = "start";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<Game1AnswerGreetings> _logger;

        public Game2AnswerGreetings(ITelegramBotClient telegramBotClient, BlobServiceClient blobServiceClient, ILogger<Game1AnswerGreetings> logger)
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
                await _telegramBotClient.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: blob.Uri.AbsoluteUri);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot send picture: {ex.Message}", ex);
                return $"Cannot send picture: {ex.Message}";
            }
            return REPLY_TEXT;
        }
    }
}

