using BerkutBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BerkutBot.Games.Game2.Infrastructure
{
	public static class ServiceCollectionExtensions
	{
        public static IServiceCollection AddGame2Services(this IServiceCollection services)
        {
            services.AddTransient<IGameAnswer, Game2AnswerIncorrect>();
            services.AddTransient<IGameAnswer, Game2AnswerLondon>();
            services.AddTransient<IGameAnswer, Game2AnswerWhiskey>();
            services.AddTransient<IGameAnswer, Game2AnswerGraffiti>();
            services.AddTransient<IGameAnswer, Game2AnswerWarsawExpress>();
            services.AddTransient<IGameAnswer, Game2AnswerVokrugSveta>();
            services.AddTransient<IGameAnswer, Game2AnswerUganda>();
            services.AddTransient<IGameAnswer, Game2AnswerFinland>();
            services.AddTransient<IGameAnswer, Game2AnswerEngland>();
            services.AddTransient<IGameAnswer, Game2AnswerFrance>();
            services.AddTransient<IGameAnswer, Game2AnswerChina>();
            services.AddTransient<IGameAnswer, Game2AnswerScotland>();
            services.AddTransient<IGameAnswer, Game2AnswerNetherlands>();
            services.AddTransient<IGameAnswer, Game2AnswerGreetings2>();
            return services;
        }
    }
}

