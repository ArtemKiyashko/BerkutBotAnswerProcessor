using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game1.Infrastructure
{
    public class Game1AnswerFactory : IGame1AnswerFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<IGame1Answer> _game1Answers;

        public Game1AnswerFactory(IServiceProvider serviceProvider, IEnumerable<IGame1Answer> game1Answers)
        {
            _serviceProvider = serviceProvider;
            _game1Answers = game1Answers;
        }

        public IGame1Answer GetInstance(Message message)
            => _game1Answers.OrderBy(answ => answ.Order).First(answ => answ.Intent(message.Text));
    }
}

