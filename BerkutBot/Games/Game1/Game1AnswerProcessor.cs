using System;
using System.Threading.Tasks;
using BerkutBot.Games.Game1.Infrastructure;
using BerkutBot.Helpers;
using BerkutBot.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game1
{
    public class Game1AnswerProcessor
    {
        private readonly IGameAnswerFactory _gameAnswerFactory;

        public Game1AnswerProcessor(IGameAnswerFactory game1AnswerFactory)
        {
            _gameAnswerFactory = game1AnswerFactory;
        }

        [FunctionName("Game1AnswerProcessor")]
        public async Task Run([ServiceBusTrigger("tgincomemessages", "game", Connection = "ServiceBusConnection", IsSessionsEnabled = true)]Message tgMessage, ILogger log)
        {
            log.LogInformation($"Message received: {tgMessage.ToJson()}");

            IGameAnswer gameAnswer = _gameAnswerFactory.GetInstance(tgMessage);

            string resultString = await gameAnswer.Reply(tgMessage);
            log.LogInformation($"Response sent: {resultString}");
        }
    }
}

