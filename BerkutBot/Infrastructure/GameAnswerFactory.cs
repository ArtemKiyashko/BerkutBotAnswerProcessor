using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;

namespace BerkutBot.Infrastructure
{
    public class GameAnswerFactory : IGameAnswerFactory
    {
        private readonly IEnumerable<IGameAnswer> _gameAnswers;

        public GameAnswerFactory(IEnumerable<IGameAnswer> gameAnswers)
        {
            _gameAnswers = gameAnswers;
        }

        public IGameAnswer GetInstance(Message message)
            => _gameAnswers.OrderBy(answ => answ.Order).First(answ => answ.Intent(message.Text));
    }
}

