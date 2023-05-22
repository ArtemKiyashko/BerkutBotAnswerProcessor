using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game4.StartCommands
{
    public class DefaultStartCommand : IStartCommand
    {
        private const string VIDEO_BLOB = "game4_prestart.mp4";
        private const string VIDEO_CONTAINER = "public";
        private const string REPLY_TEXT = "Pre-start 1 video sent";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<DefaultStartCommand> _logger;
        private readonly BlobServiceClient _blobServiceClient;

        public DefaultStartCommand(ITelegramBotClient telegramBotClient, ILogger<DefaultStartCommand> logger, BlobServiceClient blobServiceClient)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _blobServiceClient = blobServiceClient;
        }

        public Func<string, bool> Intent => (string text) => string.IsNullOrEmpty(text);

        public int Order => 999;

        public async Task<string> Reply(Message message)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(VIDEO_CONTAINER);
            BlobClient blob = container.GetBlobClient(VIDEO_BLOB);
            try
            {
                await _telegramBotClient.SendVideoAsync(
                        chatId: message.Chat.Id,
                        video: InputFile.FromUri(blob.Uri)
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DefaultStartCommand)} fails: {ex.Message}", ex);
                return $"{nameof(DefaultStartCommand)} fails: {ex.Message}";
            }
            return REPLY_TEXT;
        }
    }
}

