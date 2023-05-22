using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BerkutBot.Infrastructure;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game5
{
    public class Game5StartCommandHandler : IGameAnswer
    {
        private const string COMMAND = "/start";
        private string COMMAND_NOT_FOUND_REPLY = "Command [{0}]. No handler registered for argument [{1}]";
        private readonly IEnumerable<IStartCommand> _startCommands;

        public Game5StartCommandHandler(IEnumerable<IStartCommand> startCommands)
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

