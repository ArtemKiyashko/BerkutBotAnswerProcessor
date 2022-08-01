using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BerkutBot.Options;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using static System.Net.Mime.MediaTypeNames;

namespace BerkutBot.Games.Game1
{
    public class Game1AnswerRally : IGame1Answer
    {
        private const string REPLY_TEXT = "От памятника выдвигайтесь в сторону ближайшего кругового движения\n--> 300м прямо поворот направо\n--> 1550м прямо поворот направо\n--> 550м прямо разворот на круговом движении\n--> едем 900м после поворот направо\n--> 800м прямо и поворот в карман\n--> 300м по карману";
        private const string ANSWER = "rally";
        private const string PICTURE_CONTAINER = "public";
        private const string PICTURE_BLOB = "2022-07-25 22.45.45.jpg";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly BlobServiceClient _blobServiceClient;

        public Game1AnswerRally(ITelegramBotClient telegramBotClient, BlobServiceClient blobServiceClient)
        {
            _telegramBotClient = telegramBotClient;
            _blobServiceClient = blobServiceClient;
        }

        Func<string, bool> IGame1Answer.Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 1;

        public async Task<string> Reply(Message message)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(PICTURE_CONTAINER);
            BlobClient blob = container.GetBlobClient(PICTURE_BLOB);

            await _telegramBotClient.SendPhotoAsync(
                chatId: message.Chat.Id,
                photo: blob.Uri.AbsoluteUri,
                caption: REPLY_TEXT,
                replyToMessageId: message.MessageId);

            return REPLY_TEXT;
        }
    }
}

