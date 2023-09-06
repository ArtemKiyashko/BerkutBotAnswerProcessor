using BerkutBot.Games.Game8.StartCommands;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BerkutBot.Games.Game8.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGame8Services(this IServiceCollection services)
        {
            services.AddTransient<IGameAnswer, Game8IncorrectCommandHandler>();
            services.AddTransient<IGameAnswer, Game8StartCommandHandler>();
            services.AddTransient<IGameAnswer, Game8HelpCommandHandler>();
            services.AddTransient<IGameAnswer, Game8AnswerGo>();
            services.AddTransient<IStartCommand, DefaultStartCommand>();
            services.AddTransient<IStartCommand, UnknownStartCommand>();


            services.AddTransient<IStartCommand, Point2>();
            services.AddTransient<IStartCommand, TestPoint>();

            return services;
        }
    }
}

