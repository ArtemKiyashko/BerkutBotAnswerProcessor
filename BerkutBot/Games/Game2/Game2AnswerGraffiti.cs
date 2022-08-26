using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game2
{
	public class Game2AnswerGraffiti : IGameAnswer
	{
        private const string REPLY_TEXT = "Graffiti answer sent";
        private const string CONTAINER = "public";
        private const string PICTURE_BLOB = "image.png";
        private const string MUSIC_BLOB = "!.mp3";
        private const string ANSWER = "graffiti";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<Game2AnswerGraffiti> _logger;

        public Game2AnswerGraffiti(ITelegramBotClient telegramBotClient, BlobServiceClient blobServiceClient, ILogger<Game2AnswerGraffiti> logger)
		{
            _telegramBotClient = telegramBotClient;
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public Func<string, bool> Intent => text => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 3;

        public async Task<string> Reply(Message message)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(CONTAINER);
            BlobClient picture = container.GetBlobClient(PICTURE_BLOB);
            BlobClient music = container.GetBlobClient(MUSIC_BLOB);

            try
            {
                await _telegramBotClient.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: picture.Uri.AbsoluteUri);

                await _telegramBotClient.SendAudioAsync(
                    chatId: message.Chat.Id,
                    audio: music.Uri.AbsoluteUri,
                    replyToMessageId: message.MessageId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot send graffiti answer: {ex.Message}", ex);
                return $"Cannot send graffiti answer: {ex.Message}";
            }
            return REPLY_TEXT;
        }
    }
}

