using System;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game10.StartCommands
{
	public class TestPoint : IStartCommand
	{
        private const string ANSWER = "TestPoint_84723a12-3e57-4a83-b998-de086b761a36";

        private readonly ITelegramBotClient _telegramBotClient;

        public TestPoint(
            ITelegramBotClient telegramBotClient)
		{
            _telegramBotClient = telegramBotClient;
        }

        public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

        public int Order => 12;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendTextMessageAsync(
                message.Chat.Id, "Проверочная метка принята!\nВот так и должно выглядеть нормальное взаимодествие со мной.");

            return $"{ANSWER} sent";
        }
    }
}

