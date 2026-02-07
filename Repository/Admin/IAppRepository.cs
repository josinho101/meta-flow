using Models;

namespace Repository.Admin
{
    public interface IAppRepository
    {
        Task<App> CreateAsync(App app);
        Task<List<App>> GetAllAsync();
        Task<App> GetByIdAsync(int id);
        Task<App> GetByNameAsync(string name);
        Task<bool> UpdateAsync(int id, App app);
        Task<bool> DeleteAsync(int id);
        Task<bool> FindByNameAsync(string name);
        Task<bool> FindByIdAsync(int id);
    }
}
