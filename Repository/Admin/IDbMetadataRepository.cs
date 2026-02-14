using Models;

namespace Repository.Admin
{
    public interface IDbMetadataRepository
    {
        Task<bool> CreateDbAsync(int appId, DbMetadata dbMetadata);
        Task<bool> DeleteDbAsync(int appId);

        Task<bool> IsValidDbNameAsync(string dbName);
        Task<bool> IsValidUsernameAsync(string username);
        Task<DbMetadata> GetDbMetadataByAppNameAsync(int appId);
    }
}
