using Repository.Admin;

namespace Engine.Services.StartupService
{
    public class StartupService : IStartupService
    {
        private readonly IStartupRepository startupRepository;

        private readonly ILogger<StartupService> logger;

        public StartupService(IStartupRepository startupRepository, ILogger<StartupService> logger)
        {
            this.startupRepository = startupRepository;
            this.logger = logger;
        }

        public async Task InitAppAsync()
        {
            await startupRepository.GenarateAppTableAsync();
            logger.LogInformation("App table generation completed");

            await startupRepository.GenarateDbMetadataTableAsync();
            logger.LogInformation("DBMetadata table generation completed");

            await startupRepository.GenarateEntityTableAsync();
            logger.LogInformation("Entity table generation completed");
        }
    }
}
