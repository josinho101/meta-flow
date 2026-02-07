namespace Repository.Admin
{
    public interface IStartupRepository
    {
        Task<bool> GenarateAppTableAsync();
        Task<bool> GenarateEntityTableAsync();
        Task<bool> GenarateDbMetadataTableAsync();
    }
}
