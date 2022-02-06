using AutoMapper;
using Demo.Adventures.Api.Infrastructure;
using Demo.Adventures.Api.MapperProfiles;
using Demo.Adventures.Database;
using Demo.Adventures.Logic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Demo.Adventures.Api
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            AddAutoMapperProfiles(services);
            AddSwagger(services);

            services
                .AddLogicService()
                .AddMongoDBRepositories(Configuration);
            //.AddInMemoryRepositories() - can use in-memory repos for tests
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>(env.IsDevelopment());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void AddAutoMapperProfiles(IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile<AdventureProfile>();
                });

            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Adventure Service API",
                    Description = "Adventures are to the adventurous"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }
    }
}
