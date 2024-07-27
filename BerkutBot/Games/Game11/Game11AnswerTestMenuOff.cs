using System;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game11;

public class Game11AnswerTestMenuOff : IGameAnswer
{
    private const string ANSWER = "test menu off";
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ILogger<Game11AnswerTestMenuOff> _logger;

    public Game11AnswerTestMenuOff(ITelegramBotClient telegramBotClient,
            ILogger<Game11AnswerTestMenuOff> logger)
    {
        _telegramBotClient = telegramBotClient;
        _logger = logger;
    }

    public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

    public int Order => 100;

    public async Task<string> Reply(Message message)
    {
        var menuButtonCommands = new MenuButtonDefault();

        await _telegramBotClient.SetChatMenuButtonAsync(message.Chat.Id, menuButtonCommands);
        return "done";
    }
}
