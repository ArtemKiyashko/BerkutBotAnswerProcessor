using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text.Json;
using Microsoft.Extensions.Options;
using BerkutBot.Options;
using BerkutBot.Games.Game1.Infrastructure;
using BerkutBot.Games.Game1.Options;

namespace BerkutBot.Games.Game1
{
    public class Game1
    {
        private readonly IGame1AnswerFactory _game1AnswerFactory;
        private readonly IUpdateMessageFactory _updateMessageFatory;
        private Message _processingMessage;

        public Game1(IGame1AnswerFactory game1AnswerFactory, IUpdateMessageFactory updateMessageFatory)
        {
            _game1AnswerFactory = game1AnswerFactory;
            _updateMessageFatory = updateMessageFatory;
        }

        [FunctionName("Game1")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] Update incomingUpdate, ILogger log)
        {
            log.LogInformation($"Update received: {JsonSerializer.Serialize(incomingUpdate)}");

            _processingMessage = _updateMessageFatory.GetMessage(incomingUpdate);

            if (_processingMessage is null)
                return new OkObjectResult($"Unsupported update type {incomingUpdate.Type}");

            IGame1Answer game1Answer = _game1AnswerFactory.GetInstance(_processingMessage);

            try
            {
                string httpResponseBody = await game1Answer.Reply(_processingMessage);
                log.LogInformation($"Response sent: {httpResponseBody}");
                return new OkObjectResult(httpResponseBody);
            }
            catch(Exception ex)
            {
                log.LogError("Error processing response", ex);
                return new BadRequestObjectResult("Error processing response");
            }
        }
    }
}
