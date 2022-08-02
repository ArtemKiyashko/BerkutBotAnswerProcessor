using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Infrastructure
{
    public class GameAnswerFactory : IGameAnswerFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<IGameAnswer> _game1Answers;

        public GameAnswerFactory(IServiceProvider serviceProvider, IEnumerable<IGameAnswer> game1Answers)
        {
            _serviceProvider = serviceProvider;
            _game1Answers = game1Answers;
        }

        public IGameAnswer GetInstance(Message message)
            => _game1Answers.OrderBy(answ => answ.Order).First(answ => answ.Intent(message.Text));
    }
}

