using Repository.Admin;

namespace Engine.Services.StartupService
{
    public class StartupService : IStartupService
    {
        private readonly IMetaFlowRepository metaFlowRepository;

        public StartupService(IMetaFlowRepository metaFlowRepository)
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
