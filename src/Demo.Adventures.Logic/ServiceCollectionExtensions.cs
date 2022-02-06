using Demo.Adventures.Logic.Contracts;
using Demo.Adventures.Logic.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Adventures.Logic
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLogicService(this IServiceCollection services)
        {
            services.AddTransient<IAdventureService, AdventureService>();
            services.AddTransient<IGameService, GameService>();

            return services;
        }
    }
}