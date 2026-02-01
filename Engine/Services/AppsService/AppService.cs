using Models.Apps;
using Models.Enums;
using Repository.Admin;
using Engine.Exceptions;
using Engine.Models.ViewModels;
using Repository.Admin.Postgres;

namespace Engine.Services.AppsService
{
    public class AppService : IAppService
    {
        private readonly ILogger<AppPostgresRepository> logger;

        private readonly IAppRepository appRepository;

        public AppService(IAppRepository appRepository, ILogger<AppPostgresRepository> logger)
        {
            this.appRepository = appRepository;
            this.logger = logger;
        }

        public async Task<AppViewModel> Create(AppViewModel model)
        {
            try
            {
                App app = model.ToDao();
                if (await appRepository.FindByName(app.Name))
                {
                    throw new DuplicateEntityException("App with the same name already exists");
                }

                app.Status = (short)Status.Active;
                var result = await appRepository.Create(app);
                if (result == null)
                {
                    return null;
                }
                return result.ToViewModel();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating app");
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                if (!await appRepository.FindById(id))
                {
                    throw new EntityNotFoundException($"App with {id} not found");
                }

                var result = await appRepository.Delete(id);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error deleting app with id {id}");
                throw;
            }
        }

        public async Task<AppViewModel> Get(int id)
        {
            try
            {
                var app = await appRepository.Get(id);
                return app?.ToViewModel();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error getting app with id {id}");
                throw;
            }
        }

        public async Task<List<AppViewModel>> GetAll()
        {
            try
            {
                var apps = await appRepository.GetAll();
                return apps?.Select(app => app.ToViewModel()).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error Getting apps");
                throw;
            }
        }

        public async Task<bool> Update(int id, AppViewModel app)
        {
            try
            {
                if (await appRepository.FindByName(app.Name))
                {
                    throw new DuplicateEntityException("App with the same name already exists");
                }

                var result = await appRepository.Update(id, app.ToDao());
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
