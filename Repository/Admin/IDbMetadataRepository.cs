using Models;

namespace Repository.Admin
{
    public interface IDbMetadataRepository
    {
        Task<bool> CreateDbAsync(App app);
        Task<bool> DeleteDbAsync(App app);
        Task<DbMetadata> GetDbMetadataByAppNameAsync(string appName);
    }
}
