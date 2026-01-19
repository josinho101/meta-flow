using Engine.EntityService;
using Engine.Services.EntityService;
using MetaParsers.EntityParser;
using Validators.EntityValidator;

namespace Engine
{
    public static class AppServices
    {
        public static IServiceCollection Add(this IServiceCollection services)
        {
            services.AddSingleton<IEntityParser<string>, StringEntityParser>();
            services.AddSingleton<IEntityValidator, EntityValidator>();
            services.AddSingleton<IEntityService, Engine.Services.EntityService.EntityService>();

            return services;
        }
    }
}
