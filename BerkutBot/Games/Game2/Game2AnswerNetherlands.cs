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
	public class Game2AnswerNetherlands : IGameAnswer
	{
        private const string ANSWER_ID = "Netherlands answer";
        private const string REPLY_TEXT = $"{ANSWER_ID} sent";
        private const string CONTAINER = "public";
        private const string PICTURE_BLOB = "netherlands.jpg";
        private readonly HashSet<string> _answerSet = new() { "нидерланды", "netherlands", "netherland" };

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<Game2AnswerNetherlands> _logger;

        public Game2AnswerNetherlands(ITelegramBotClient telegramBotClient, BlobServiceClient blobServiceClient, ILogger<Game2AnswerNetherlands> logger)
		{
            _telegramBotClient = telegramBotClient;
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public Func<string, bool> Intent =>
            text =>
            _answerSet.Any(ans => ans.Equals(text, StringComparison.OrdinalIgnoreCase));

        public int Order => 13;

        public async Task<string> Reply(Message message)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(CONTAINER);
            BlobClient picture = container.GetBlobClient(PICTURE_BLOB);

            try
            {
                await _telegramBotClient.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: InputFile.FromUri(picture.Uri),
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

