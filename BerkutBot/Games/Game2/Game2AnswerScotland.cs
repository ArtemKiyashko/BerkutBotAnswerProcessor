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
	public class Game2AnswerScotland : IGameAnswer
	{
        private const string VIDEO_CAPTION = "Скоростной участок!!!!! Мчите на финиш на парковку аутлета на Таллинском шоссе!!!!!";
        private const string ANSWER_ID = "Scotland answer";
        private const string REPLY_TEXT = $"{ANSWER_ID} sent";
        private const string CONTAINER = "public";
        private const string VIDEO_BLOB = "rally2.mp4";
        private readonly HashSet<string> _answerSet = new() { "scotland", "шотландия"};

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<Game2AnswerScotland> _logger;

        public Game2AnswerScotland(ITelegramBotClient telegramBotClient, BlobServiceClient blobServiceClient, ILogger<Game2AnswerScotland> logger)
		{
            _telegramBotClient = telegramBotClient;
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public Func<string, bool> Intent =>
            text =>
            _answerSet.Any(ans => ans.Equals(text, StringComparison.OrdinalIgnoreCase));

        public int Order => 12;

        public async Task<string> Reply(Message message)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(CONTAINER);
            BlobClient video = container.GetBlobClient(VIDEO_BLOB);

            try
            {
                await _telegramBotClient.SendVideoAsync(
                    chatId: message.Chat.Id,
                    video: InputFile.FromUri(video.Uri),
                    caption: VIDEO_CAPTION);
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

