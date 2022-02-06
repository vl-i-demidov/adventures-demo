using Demo.Adventures.Database.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Adventures.Tests.Mocks.Database
{
    public static class ServiceCollectionExtensions
    {
        // ReSharper disable once InconsistentNaming
        public static IServiceCollection AddInMemoryRepositories(this IServiceCollection services)
        {
            services.AddTransient<IAdventureRepository, AdventureRepositoryMock>();
            services.AddTransient<IStepRepository, StepRepositoryMock>();
            services.AddTransient<IGameRepository, GameRepositoryMock>();

            return services;
        }
    }
}