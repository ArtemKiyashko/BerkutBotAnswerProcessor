using System;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game11;

public class Game11AnswerTestMenuOn : IGameAnswer
{
    private const string ANSWER = "test menu on";
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ILogger<Game11AnswerTestMenuOn> _logger;

    public Game11AnswerTestMenuOn(ITelegramBotClient telegramBotClient,
            ILogger<Game11AnswerTestMenuOn> logger)
    {
        _telegramBotClient = telegramBotClient;
        _logger = logger;
    }

    public Func<string, bool> Intent => (string text) => ANSWER.Equals(text, StringComparison.OrdinalIgnoreCase);

    public int Order => 100;

    public async Task<string> Reply(Message message)
    {
        var menuButtonLamp = new MenuButtonWebApp
        {
            WebApp = new WebAppInfo { Url = "https://sawevprivate.z6.web.core.windows.net" },
            Text = "💡"
        };
        await _telegramBotClient.SetChatMenuButtonAsync(message.Chat.Id, menuButtonLamp);
        return "done";
    }
}
