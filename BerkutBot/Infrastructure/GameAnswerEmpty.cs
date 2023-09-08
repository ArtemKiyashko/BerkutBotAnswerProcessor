using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BerkutBot.Infrastructure
{
	public class GameAnswerEmpty : IGameAnswer
	{

        public Func<string, bool> Intent => (text) => true;

        public int Order => 0;

        public async Task<string> Reply(Message message) => string.Empty;
    }
}

