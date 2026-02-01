using Models;

namespace Repository.Admin
{
    public interface IAppRepository
    {
        Task<App> Create(App app);
        Task<List<App>> GetAll();
        Task<App> Get(int id);
        Task<bool> Update(int id, App app);
        Task<bool> Delete(int id);
        Task<bool> FindByName(string name);
        Task<bool> FindById(int id);
    }
}
