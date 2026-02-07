using Models.Enums;
using Repository.Admin;
using Engine.Exceptions;
using Engine.Models.ViewModels;
using Repository.Admin.Postgres;
using Models;

namespace Engine.Services.AppsService
{
    public class AppService : IAppService
    {
        private readonly ILogger<AppRepository> logger;

        private readonly IAppRepository appRepository;

        private readonly IDbMetadataRepository dbMetadataRepository;

        public AppService(ILogger<AppRepository> logger, IAppRepository appRepository, IDbMetadataRepository dbMetadataRepository)
        {
            this.logger = logger;
            this.appRepository = appRepository;
            this.dbMetadataRepository = dbMetadataRepository;
        }

        public async Task<AppViewModel> CreateAsync(AppViewModel model)
        {
            try
            {
                App app = model.ToDao();
                if (await appRepository.FindByNameAsync(app.Name))
                {
                    throw new DuplicateEntityException("App with the same name already exists");
                }

                app.Status = (short)Status.Active;
                var result = await appRepository.CreateAsync(app);
                logger.LogInformation($"App {app.Name} created");

                // create database and user for the app
                await dbMetadataRepository.CreateDbAsync(app);
                logger.LogInformation($"App database and user created");

                return result.ToViewModel();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating app");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {

                var app = await appRepository.GetByIdAsync(id);
                if (app == null)
                {
                    throw new EntityNotFoundException($"App with {id} not found");
                }

                // delete database and user for the app
                await dbMetadataRepository.DeleteDbAsync(app);
                logger.LogInformation($"App {app.Name} database and user deleted");

                var result = await appRepository.DeleteAsync(id);
                logger.LogInformation($"App {app.Name} deleted");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error deleting app with id {id}");
                throw;
            }
        }

        public async Task<AppViewModel?> GetAsync(int id)
        {
            try
            {
                var app = await appRepository.GetByIdAsync(id);
                return app?.ToViewModel();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error getting app with id {id}");
                throw;
            }
        }

        public async Task<List<AppViewModel>> GetAllAsync()
        {
            try
            {
                var apps = await appRepository.GetAllAsync();
                return apps?.Select(app => app.ToViewModel()).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error Getting apps");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, AppViewModel app)
        {
            try
            {
                if (await appRepository.FindByNameAsync(app.Name))
                {
                    throw new DuplicateEntityException("App with the same name already exists");
                }

                var result = await appRepository.UpdateAsync(id, app.ToDao());
                logger.LogInformation($"App with {id} updated", app);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error updating app with id {id}");
                throw;
            }
        }
    }
}
