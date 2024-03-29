﻿using System;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Telegram.Bot;
using BerkutBot.Infrastructure;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game2
{
	public class Game2AnswerGreetings : IGameAnswer
	{
        private const string REPLY_TEXT = "Короче, {0}, я тебя спас и в благородство играть не буду: выполнишь для меня пару заданий — и мы в расчете." +
            "\nЗаодно посмотрим, как быстро у тебя башка после амнезии прояснится. А по твоей теме постараюсь разузнать." +
            "\nХрен его знает, на кой ляд тебе этот Беркут сдался, но скоро ты узнаешь, откуда начнется приключение...";
        private const string ANSWER = "start";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Game2AnswerGreetings> _logger;

        public Game2AnswerGreetings(ITelegramBotClient telegramBotClient, ILogger<Game2AnswerGreetings> logger)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
        }

        public Func<string, bool> Intent => FormatMessage;

        public int Order => 2;

        private bool FormatMessage(string text)
            => !string.IsNullOrEmpty(text) && text.Replace("/", "").Equals(ANSWER, StringComparison.OrdinalIgnoreCase);

        public async Task<string> Reply(Message message)
        {
            try
            {
                var replyFormatted = string.Format(REPLY_TEXT, message.From.FirstName ?? message.From.Username);
                await _telegramBotClient.SendTextMessageAsync(
                    message.Chat.Id,
                    text: replyFormatted);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot send picture: {ex.Message}", ex);
                return $"Cannot send picture: {ex.Message}";
            }
            return REPLY_TEXT;
        }
    }
}

