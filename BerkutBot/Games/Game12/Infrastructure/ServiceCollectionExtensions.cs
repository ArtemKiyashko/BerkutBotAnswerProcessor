using BerkutBot.Games.Game12.StartCommands;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BerkutBot.Games.Game12.Infrastructure;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddGame12Services(this IServiceCollection services)
    {
        services.AddTransient<IGameAnswer, Game12AnswerGo>();
        services.AddTransient<IGameAnswer, Game12AnswerWantMore>();
        services.AddTransient<IGameAnswer, Game12AnswerRally>();
        services.AddTransient<IGameAnswer, Game12AnswerLenta>();

        services.AddTransient<IStartCommand, TestPoint>();
        services.AddTransient<IStartCommand, ArQr>();
        services.AddTransient<IStartCommand, Point1>();
        services.AddTransient<IStartCommand, Point3>();
        services.AddTransient<IStartCommand, Point4>();
        services.AddTransient<IStartCommand, Point6>();
        services.AddTransient<IStartCommand, Point7>();
        services.AddTransient<IStartCommand, Point8>();
        services.AddTransient<IStartCommand, Point9>();
        services.AddTransient<IStartCommand, Point11>();
        services.AddTransient<IStartCommand, Point12>();
        return services;
    }
}
