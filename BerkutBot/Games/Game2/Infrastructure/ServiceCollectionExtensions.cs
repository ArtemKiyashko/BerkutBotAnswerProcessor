using BerkutBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BerkutBot.Games.Game2.Infrastructure
{
	public static class ServiceCollectionExtensions
	{
        public static IServiceCollection AddGame2Services(this IServiceCollection services)
        {
            services.AddTransient<IGameAnswer, Game2AnswerIncorrect>();
            services.AddTransient<IGameAnswer, Game2AnswerGreetings>();
            return services;
        }
    }
}

