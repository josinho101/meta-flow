using Engine.Models.ViewModels;

namespace Engine.Services.AppsService
{
    public interface IAppService
    {
        public Task<AppViewModel> CreateAsync(AppViewModel app);
        public Task<List<AppViewModel>> GetAllAsync();
        public Task<AppViewModel> GetAsync(int id);
        public Task<bool> UpdateAsync(int id, AppViewModel app);
        public Task<bool> DeleteAsync(int id);
    }
}
