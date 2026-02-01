using Repository.Admin;

namespace Engine.Services.StartupService
{
    public class StartupService : IStartupService
    {
        private readonly IStartupRepository metaFlowRepository;

        private readonly ILogger<StartupService> logger;

        public StartupService(IStartupRepository metaFlowRepository, ILogger<StartupService> logger)
        {
            this.metaFlowRepository = metaFlowRepository;
            this.logger = logger;
        }

        public async Task InitApp()
        {
            await metaFlowRepository.GenarateAppTable();
            logger.LogInformation("App table generation completed");
            await metaFlowRepository.GenarateEntityTable();
            logger.LogInformation("Entity table generation completed");
        }
    }
}
