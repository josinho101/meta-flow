using Repository.Admin;

namespace Engine.Services.StartupService
{
    public class StartupService : IStartupService
    {
        private readonly IStartupRepository metaFlowRepository;

        public StartupService(IStartupRepository metaFlowRepository)
        {
            this.metaFlowRepository = metaFlowRepository;
        }

        public async Task InitApp()
        {
            await metaFlowRepository.GenarateAppTable();
            await metaFlowRepository.GenarateEntityTable();
        }
    }
}
