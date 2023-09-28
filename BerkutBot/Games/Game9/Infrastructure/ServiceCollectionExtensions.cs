using BerkutBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using BerkutBot.Games.Game9.StartCommands;

namespace BerkutBot.Games.Game9.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGame9Services(this IServiceCollection services)
        {
            services.AddTransient<IGameAnswer, Game9IncorrectCommandHandler>();
            services.AddTransient<IGameAnswer, Game9StartCommandHandler>();
            services.AddTransient<IGameAnswer, Game9HelpCommandHandler>();
            services.AddTransient<IStartCommand, DefaultStartCommand>();
            services.AddTransient<IStartCommand, UnknownStartCommand>();
            services.AddTransient<IGameAnswer, Game9AnswerGo>();


            services.AddTransient<IStartCommand, Point5>();
            services.AddTransient<IStartCommand, Point6>();
            services.AddTransient<IStartCommand, Point7>();
            services.AddTransient<IStartCommand, TestPoint>();

            return services;
        }
    }
}

