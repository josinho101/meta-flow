using Models;

namespace Repository.Admin
{
    public interface IDbMetadataRepository
    {
        Task<bool> CreateDbAsync(int appId, DbMetadata dbMetadata);
        Task<bool> DeleteDbAsync(int appId);
        Task<DbMetadata> GetDbMetadataByAppNameAsync(int appId);
    }
}
