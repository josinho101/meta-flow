using Engine.Models.ViewModels;

namespace Engine.Services.AppsService
{
    public interface IAppService
    {
        public Task<AppViewModel> Create(AppViewModel app);
        public Task<List<AppViewModel>> GetAll();
        public Task<AppViewModel> Get(int id);
        public Task<bool> Update(int id, AppViewModel app);
        public Task<bool> Delete(int id);
    }
}
