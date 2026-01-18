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

            return services;
        }
    }
}
