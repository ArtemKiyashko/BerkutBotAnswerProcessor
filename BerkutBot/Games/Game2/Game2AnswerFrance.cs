using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game2
{
	public class Game2AnswerFrance : IGameAnswer
	{
        private const string ANSWER_ID = "France answer";
        private const string REPLY_TEXT = $"{ANSWER_ID} sent";
        private const string CONTAINER = "public";
        private const string PICTURE_BLOB = "france.jpg";
        private readonly HashSet<string> _answerSet = new() { "france", "франция" };

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<Game2AnswerFrance> _logger;

        public Game2AnswerFrance(ITelegramBotClient telegramBotClient, BlobServiceClient blobServiceClient, ILogger<Game2AnswerFrance> logger)
		{
            _telegramBotClient = telegramBotClient;
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public Func<string, bool> Intent =>
            text =>
            _answerSet.Any(ans => ans.Equals(text, StringComparison.OrdinalIgnoreCase));

        public int Order => 11;

        public async Task<string> Reply(Message message)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(CONTAINER);
            BlobClient picture = container.GetBlobClient(PICTURE_BLOB);

            try
            {
                await _telegramBotClient.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: picture.Uri.AbsoluteUri,
                    replyToMessageId: message.MessageId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot send {ANSWER_ID}: {ex.Message}", ex);
                return $"Cannot send {ANSWER_ID}: {ex.Message}";
            }
            return REPLY_TEXT;
        }
    }
}

