using System;
using System.Threading.Tasks;
using BerkutBot.Games.Game1.Infrastructure;
using BerkutBot.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game1
{
    public class Game1AnswerProcessor
    {
        private readonly IGame1AnswerFactory _game1AnswerFactory;

        public Game1AnswerProcessor(IGame1AnswerFactory game1AnswerFactory)
        {
            _game1AnswerFactory = game1AnswerFactory;
        }

        [FunctionName("Game1AnswerProcessor")]
        public async Task Run([ServiceBusTrigger("tgincomemessages", "game", Connection = "ServiceBusConnection", IsSessionsEnabled = true)]Message tgMessage, ILogger log)
        {
            log.LogInformation($"Message received: {tgMessage.ToJson()}");

            IGame1Answer game1Answer = _game1AnswerFactory.GetInstance(tgMessage);

            string resultString = await game1Answer.Reply(tgMessage);
            log.LogInformation($"Response sent: {resultString}");
        }
    }
}

