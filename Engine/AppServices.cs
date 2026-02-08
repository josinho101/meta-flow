using Engine.EntityService;
using Engine.Services.AppsService;
using Engine.Services.StartupService;
using MetaParsers.EntityParser;
using Repository.Admin;
using Repository.Postgres.Admin;
using Repository.Postgres.Application;
using Validators.EntityValidator;

namespace Engine
{
    public static class AppServices
    {
        public static IServiceCollection Add(this IServiceCollection services)
        {
            services.AddSingleton<IEntityParser<string>, StringEntityParser>();
            services.AddSingleton<IEntityValidator, EntityValidator>();
            services.AddSingleton<IEntityRepository, EntityRepository>();
            services.AddSingleton<IEntityService, Services.EntityService.EntityService>();

            services.AddSingleton<IMetaFlowRepository, MetaFlowRepository>();
            services.AddSingleton<IAppDbRepository, AppDbRepository>();

            services.AddSingleton<IStartupService, StartupService>();
            services.AddSingleton<IStartupRepository, StartupRepository>();

            services.AddSingleton<IAppService, AppService>();
            services.AddSingleton<IAppRepository, AppRepository>();

            services.AddSingleton<IDbMetadataRepository, DbMetadataRepository>();

            return services;
        }
    }
}
