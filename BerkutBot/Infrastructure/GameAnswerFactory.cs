using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;

namespace BerkutBot.Infrastructure
{
    public class GameAnswerFactory : IGameAnswerFactory
    {
        private readonly IEnumerable<IGameAnswer> _game1Answers;

        public GameAnswerFactory(IEnumerable<IGameAnswer> game1Answers)
        {
            _game1Answers = game1Answers;
        }

        public IGameAnswer GetInstance(Message message)
            => _game1Answers.OrderBy(answ => answ.Order).First(answ => answ.Intent(message.Text));
    }
}

