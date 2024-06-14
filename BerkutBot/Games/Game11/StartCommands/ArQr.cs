using System;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BerkutBot.Games.Game11.StartCommands
{
	public class ArQr : IStartCommand
	{
        private const string ANSWER = "ArQrPoint_bdf25cdf-37a6-4bb3-8a03-4ad44495b03a";

        private readonly ITelegramBotClient _telegramBotClient;

        public ArQr(
            ITelegramBotClient telegramBotClient)
		{
            _telegramBotClient = telegramBotClient;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 11;

        public async Task<string> Reply(Message message)
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                InlineKeyboardButton.WithUrl(
                    text: "ЖМИ!",
                    url: "https://berkut.ar")
            });

            await _telegramBotClient.SendTextMessageAsync(
                message.Chat.Id,
                "ЕКАРНЫЙ БАБАЙ!",
                replyMarkup: inlineKeyboard);

            return $"{ANSWER} sent";
        }
    }
}

