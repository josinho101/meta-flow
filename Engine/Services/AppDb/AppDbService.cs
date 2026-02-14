using Engine.Exceptions;
using Engine.Models.ViewModels;
using Models;
using Repository.Admin;

namespace Engine.Services.AppDb
{
    public class AppDbService : IAppDbService
    {
        private readonly ILogger<AppDbService> logger;

        private readonly IDbMetadataRepository dbMetadataRepository;

        private readonly IAppRepository appRepository;

        public AppDbService(ILogger<AppDbService> logger, IDbMetadataRepository dbMetadataRepository, IAppRepository appRepository)
        {
            this.logger = logger;
            this.dbMetadataRepository = dbMetadataRepository;
            this.appRepository = appRepository;
        }

        public async Task<bool> CreateDbAsync(string appName, DbMetadataViewModel viewModel)
        {
            var app = await appRepository.GetByNameAsync(appName);
            if (app == null)
            {
                throw new EntityNotFoundException($"App with name {appName} not found.");
            }

            await dbMetadataRepository.CreateDbAsync(app.Id, viewModel.ToDao());
            logger.LogInformation($"App database {viewModel.DbName} and user created");
            return true;
        }

        public async Task<bool> DeleteDbAsync(string appName)
        {
            var app = await appRepository.GetByNameAsync(appName);
            if (app == null)
            {
                throw new EntityNotFoundException($"App with name {appName} not found.");
            }

            // delete database and user for the app
            await dbMetadataRepository.DeleteDbAsync(app.Id);
            logger.LogInformation($"App {appName} database and user deleted");
            return true;
        }
    }
}
