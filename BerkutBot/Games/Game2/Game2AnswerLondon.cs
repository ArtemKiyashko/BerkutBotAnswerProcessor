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
	public class Game2AnswerLondon : IGameAnswer
	{
        private const string REPLY_TEXT = "London answer sent";
        private const string CONTAINER = "public";
        private const string MUSIC_BLOB = "Italy.mp3";
        private readonly HashSet<string> _answerSet = new() { "италия", "italy" };

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<Game2AnswerLondon> _logger;

        public Game2AnswerLondon(ITelegramBotClient telegramBotClient, BlobServiceClient blobServiceClient, ILogger<Game2AnswerLondon> logger)
		{
            _telegramBotClient = telegramBotClient;
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public Func<string, bool> Intent =>
            text =>
            _answerSet.Any(ans => ans.Equals(text, StringComparison.OrdinalIgnoreCase));

        public int Order => 3;

        public async Task<string> Reply(Message message)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(CONTAINER);
            BlobClient music = container.GetBlobClient(MUSIC_BLOB);

            try
            {
                await _telegramBotClient.SendVoiceAsync(
                    chatId: message.Chat.Id,
                    voice: InputFile.FromUri(music.Uri.AbsoluteUri),
                    replyToMessageId: message.MessageId,
                    caption: "Ну, здравствуй");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot send london answer: {ex.Message}", ex);
                return $"Cannot send london answer: {ex.Message}";
            }
            return REPLY_TEXT;
        }
    }
}

