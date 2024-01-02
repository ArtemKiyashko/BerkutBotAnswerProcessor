using BerkutBot.Games.Game10.StartCommands;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BerkutBot.Games.Game10.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGame10Services(this IServiceCollection services)
        {
            services.AddTransient<IStartCommand, TestPoint>();
            services.AddTransient<IStartCommand, Point1>();
            services.AddTransient<IStartCommand, Point4>();
            services.AddTransient<IStartCommand, Point5>();
            services.AddTransient<IStartCommand, Point6>();
            services.AddTransient<IStartCommand, Point7>();

            services.AddTransient<IGameAnswer, Game10AnswerGo>();
            services.AddTransient<IGameAnswer, Game10AnswerKiev>();
            services.AddTransient<IGameAnswer, Game10AnswerSport>();
            services.AddTransient<IGameAnswer, Game10AnswerBonuses>();
            return services;
        }
    }
}

