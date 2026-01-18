using MetaParsers.EntityParser;

namespace Engine
{
    public static class Services
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IEntityParser<string>, JsonEntityParser>();

            return services;
        }
    }
}
