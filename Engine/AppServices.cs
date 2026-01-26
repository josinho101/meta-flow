using Engine.EntityService;
using Engine.Services.StartupService;
using MetaParsers.EntityParser;
using Repository.Admin;
using Repository.Base;
using Validators.EntityValidator;

namespace Engine
{
    public static class AppServices
    {
        public static IServiceCollection Add(this IServiceCollection services)
        {
            services.AddSingleton<IEntityParser<string>, StringEntityParser>();
            services.AddSingleton<IEntityValidator, EntityValidator>();
            services.AddSingleton<IEntityService, Services.EntityService.EntityService>();
            services.AddSingleton<IDatabaseDialect, PostgresDialect>();
            services.AddSingleton<IMetaFlowRepository, MetaFlowRepository>();
            services.AddSingleton<IStartupService, StartupService>();

            return services;
        }
    }
}
