using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using static System.Net.Mime.MediaTypeNames;

namespace BerkutBot.Games.Game1
{
    public class Game1Answer8_9 : IGameAnswer
    {
        private const string REPLY_TEXT = "Song sent";
        private const string SONG_CONTAINER = "public";
        private const string SONG_BLOB = "song1.mp3";
        private const double ANSWER = 8.9;

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<Game1AnswerVideo1> _logger;

        public Game1Answer8_9(ITelegramBotClient telegramBotClient, BlobServiceClient blobServiceClient, ILogger<Game1AnswerVideo1> logger)
        {
            _telegramBotClient = telegramBotClient;
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public int Order => 4;

        Func<string, bool> IGameAnswer.Intent
            => (string text)
            => !string.IsNullOrEmpty(text) && Double.TryParse(text.Replace(',', '.'), out double result) && ANSWER.Equals(result);

        public async Task<string> Reply(Message message)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(SONG_CONTAINER);
            BlobClient blob = container.GetBlobClient(SONG_BLOB);

            var stream = await blob.DownloadStreamingAsync();

            var file = InputFile.FromStream(stream.Value.Content);
            try
            {
                await _telegramBotClient.SendVoiceAsync(
                    chatId: message.Chat.Id,
                    voice: file,
                    caption: "🗽",
                    replyToMessageId: message.MessageId);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Cannot send voice: {ex.Message}", ex);
                return $"Cannot send voice: {ex.Message}";
            }

            return REPLY_TEXT;
        }
    }
}

