using System;
using BerkutBot.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using System.Linq;

namespace BerkutBot.Games.Game4
{
    public class Game4Greetings : IGameAnswer
    {
        private const string COMMAND = "/start";
        private string COMMAND_NOT_FOUND_REPLY = "Command [{0}]. No handler registered for argument [{1}]";
        private readonly IEnumerable<IStartCommand> _startCommands;

        public Game4Greetings(IEnumerable<IStartCommand> startCommands)
        {
            _startCommands = startCommands;
        }

        public Func<string, bool> Intent => (string text) => text.StartsWith(COMMAND, StringComparison.OrdinalIgnoreCase);

        public int Order => 1;

        public async Task<string> Reply(Message message)
        {
            message.Text = message.Text.Replace(COMMAND, "").Trim();
            var startCommand = _startCommands.OrderBy(answ => answ.Order).First(answ => answ.Intent(message.Text));
            return await startCommand.Reply(message);
        }
    }
}

