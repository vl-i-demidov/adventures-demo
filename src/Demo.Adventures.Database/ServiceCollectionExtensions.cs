using Demo.Adventures.Database.Contracts;
using Demo.Adventures.Database.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Adventures.Database
{
    public static class ServiceCollectionExtensions
    {
        // ReSharper disable once InconsistentNaming
        public static IServiceCollection AddMongoDBRepositories(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<RepositoryConfig>()
                .Bind(configuration.GetSection(RepositoryConfig.ConfigurationSectionName));

            services.AddTransient<IAdventureRepository, AdventureRepository>();
            services.AddTransient<IStepRepository, StepRepository>();
            services.AddTransient<IGameRepository, GameRepository>();

            return services;
        }
    }
}