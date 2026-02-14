using Engine.EntityService;
using Engine.Services.AppDb;
using Engine.Services.AppEntityService;
using Engine.Services.AppService;
using Engine.Services.StartupService;
using MetaParsers.EntityParser;
using Models;
using Repository.Admin;
using Repository.Application;
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
            services.AddScoped<IAppDbRepository, AppDbRepository>();

            services.AddSingleton<IStartupService, StartupService>();
            services.AddSingleton<IStartupRepository, StartupRepository>();

            services.AddSingleton<IAppService, AppService>();
            services.AddSingleton<IAppRepository, AppRepository>();

            services.AddSingleton<IDbMetadataRepository, DbMetadataRepository>();

            services.AddScoped<IAppEntityService, AppEntityService>();
            services.AddScoped<IAppEntityRepository, AppEntityRepository>();

            services.AddSingleton<IAppDbService, AppDbService>();

            services.AddScoped<DbMetadata>(sp =>
            {
                IHttpContextAccessor httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                IDbMetadataRepository dbMetadataRepository = sp.GetRequiredService<IDbMetadataRepository>();
                IAppRepository appRepository = sp.GetRequiredService<IAppRepository>();

                var routeValues = httpContextAccessor.HttpContext?.Request.RouteValues;
                string appName = routeValues?["appName"]?.ToString() ?? string.Empty;

                var dbMetadata = new DbMetadata();
                if (!string.IsNullOrEmpty(appName))
                {
                    var app = appRepository.GetByNameAsync(appName).Result;
                    dbMetadata = dbMetadataRepository.GetDbMetadataByAppNameAsync(app.Id).Result;
                }

                return dbMetadata;
            });

            return services;
        }
    }
}
