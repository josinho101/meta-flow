using Models.Enums;
using Engine.Exceptions;
using Engine.Models.ViewModels;
using Models;
using Repository.Postgres.Admin;
using Repository.Admin;

namespace Engine.Services.AppService
{
    public class AppService : IAppService
    {
        private readonly ILogger<AppRepository> logger;

        private readonly IAppRepository appRepository;        

        public AppService(ILogger<AppRepository> logger, IAppRepository appRepository)
        {
            this.logger = logger;
            this.appRepository = appRepository;
        }

        public async Task<AppViewModel> CreateAsync(AppViewModel model)
        {

            App app = model.ToDao();
            if (await appRepository.FindByNameAsync(app.Name))
            {
                throw new DuplicateEntityException("App with the same name already exists");
            }

            app.Status = (short)Status.Active;
            var result = await appRepository.CreateAsync(app);
            logger.LogInformation($"App {app.Name} created");

            return result.ToViewModel();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var app = await appRepository.GetByIdAsync(id);
            if (app == null)
            {
                throw new EntityNotFoundException($"App with {id} not found");
            }

            var result = await appRepository.DeleteAsync(id);
            logger.LogInformation($"App {app.Name} deleted");
            return result;
        }

        public async Task<AppViewModel?> GetAsync(int id)
        {
            var app = await appRepository.GetByIdAsync(id);
            return app?.ToViewModel();
        }

        public async Task<List<AppViewModel>> GetAllAsync()
        {
            var apps = await appRepository.GetAllAsync();
            return apps?.Select(app => app.ToViewModel()).ToList();
        }

        public async Task<bool> UpdateAsync(int id, AppViewModel app)
        {
            if (await appRepository.FindByNameAsync(app.Name))
            {
                throw new DuplicateEntityException("App with the same name already exists");
            }

            var result = await appRepository.UpdateAsync(id, app.ToDao());
            logger.LogInformation($"App with {id} updated", app);
            return result;
        }
    }
}
