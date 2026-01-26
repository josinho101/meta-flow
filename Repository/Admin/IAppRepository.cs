using Models.Apps;

namespace Repository.Admin
{
    public interface IAppRepository
    {
        public Task<App> Create(App app);
        public Task<List<App>> GetAll();
        public Task<App> Get(int id);
        public Task<bool> Update(int id, App app);
        public Task<bool> Delete(int id);
    }
}
