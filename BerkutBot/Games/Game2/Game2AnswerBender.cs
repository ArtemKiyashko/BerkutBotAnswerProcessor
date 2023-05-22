using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game2
{
	public class Game2AnswerBender : IGameAnswer
	{
        private const string REPLY_TEXT = "Bender answer sent";
        private const string CONTAINER = "public";
        private const string VIDEO_BLOB = "BenderCall_TapSound.mp4";
        private readonly HashSet<string> _answerSet = new() { "bender", "бендер" };

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<Game2AnswerBender> _logger;

        public Game2AnswerBender(ITelegramBotClient telegramBotClient, BlobServiceClient blobServiceClient, ILogger<Game2AnswerBender> logger)
		{
            _telegramBotClient = telegramBotClient;
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public Func<string, bool> Intent =>
            text =>
            _answerSet.Any(ans => ans.Equals(text, StringComparison.OrdinalIgnoreCase));

        public int Order => 6;

        public async Task<string> Reply(Message message)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(CONTAINER);
            BlobClient video = container.GetBlobClient(VIDEO_BLOB);

            try
            {
                await _telegramBotClient.SendVideoNoteAsync(
                    chatId: message.Chat.Id,
                    videoNote: InputFile.FromUri(video.Uri.AbsoluteUri),
                    replyToMessageId: message.MessageId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot send bender answer: {ex.Message}", ex);
                return $"Cannot send bender answer: {ex.Message}";
            }
            return REPLY_TEXT;
        }
    }
}

