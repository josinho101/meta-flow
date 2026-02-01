namespace Repository.Admin
{
    public interface IDbMetadataRepository
    {
        Task<bool> CreateDb(string name);
        Task<bool> DeleteDb(string name);
    }
}
