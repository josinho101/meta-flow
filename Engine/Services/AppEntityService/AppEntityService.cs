using Models.Entity;
using Repository.Application;

namespace Engine.Services.AppEntityService
{
    public class AppEntityService : IAppEntityService
    {
        private readonly ILogger<AppEntityService> logger;

        private readonly IAppEntityRepository appEntityRepository;

        public AppEntityService(ILogger<AppEntityService> logger, IAppEntityRepository appEntityRepository)
        {
            this.logger = logger;
            this.appEntityRepository = appEntityRepository;
        }

        public async Task<bool> CreateAppEntityAsync(Entity entity)
        {
            string sql = appEntityRepository.GenerateSqlScriptAsync(entity);
            logger.LogInformation($"Entity SQL Script {sql}");
            await appEntityRepository.ApplySqlScriptAsync(sql);
            logger.LogInformation("Entity SQL generated and applied to app database");
            return true;
        }
    }
}
