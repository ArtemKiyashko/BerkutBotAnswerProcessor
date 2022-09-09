using System;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Telegram.Bot;
using BerkutBot.Infrastructure;
using Telegram.Bot.Types;
using System.Reflection.Metadata;
using static System.Reflection.Metadata.BlobBuilder;

namespace BerkutBot.Games.Game2
{
	public class Game2AnswerGreetings2 : IGameAnswer
	{
        private const string REPLY_TEXT = "Назови мне это место";
        private const string PICTURE_CONTAINER = "public";
        private const string PICTURE_BLOB_0 = "vokzal1_1.jpg";
        private const string PICTURE_BLOB_1 = "HogwartsExpress.jpg";
        private const string ANSWER = "start";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<Game2AnswerGreetings> _logger;

        public Game2AnswerGreetings2(ITelegramBotClient telegramBotClient, BlobServiceClient blobServiceClient, ILogger<Game2AnswerGreetings> logger)
        {
            _telegramBotClient = telegramBotClient;
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public Func<string, bool> Intent => FormatMessage;

        public int Order => 2;

        private bool FormatMessage(string text)
            => !string.IsNullOrEmpty(text) && text.Replace("/", "").Equals(ANSWER, StringComparison.OrdinalIgnoreCase);

        public async Task<string> Reply(Message message)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(PICTURE_CONTAINER);
            BlobClient blob0 = container.GetBlobClient(PICTURE_BLOB_0);
            BlobClient blob1 = container.GetBlobClient(PICTURE_BLOB_1);

            var album = new IAlbumInputMedia[]
            {
                new InputMediaPhoto(blob0.Uri.AbsoluteUri),
                new InputMediaPhoto(blob1.Uri.AbsoluteUri) { Caption = REPLY_TEXT },
            };
            
            try
            {
                await _telegramBotClient.SendMediaGroupAsync(
                    chatId: message.Chat.Id,
                    media: album);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot send pictureы: {ex.Message}", ex);
                return $"Cannot send pictureы: {ex.Message}";
            }
            return REPLY_TEXT;
        }
    }
}

