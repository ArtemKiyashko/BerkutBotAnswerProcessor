using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using BerkutBot.Helpers;

namespace BerkutBot.Infrastructure
{
	public class GameAnswerProcessor
	{
        private readonly IGameAnswerFactory _gameAnswerFactory;

        public GameAnswerProcessor(IGameAnswerFactory game1AnswerFactory)
        {
            _gameAnswerFactory = game1AnswerFactory;
        }

        [FunctionName("GameAnswerProcessor")]
        public async Task Run([ServiceBusTrigger("tgincomemessages", "game", Connection = "ServiceBusConnection", IsSessionsEnabled = true)] Message tgMessage, ILogger log)
        {
            log.LogInformation($"Message received: {tgMessage.ToJson()}");

            IGameAnswer gameAnswer = _gameAnswerFactory.GetInstance(tgMessage);

            string resultString = await gameAnswer.Reply(tgMessage);
            log.LogInformation($"Response sent: {resultString}");
        }
    }
}

