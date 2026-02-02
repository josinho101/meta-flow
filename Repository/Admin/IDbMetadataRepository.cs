using Models;

namespace Repository.Admin
{
    public interface IDbMetadataRepository
    {
        Task<bool> CreateDb(App app);
        Task<bool> DeleteDb(App app);
    }
}
