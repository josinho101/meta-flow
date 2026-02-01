namespace Repository.Admin
{
    public interface IStartupRepository
    {
        Task<bool> GenarateAppTable();
        Task<bool> GenarateEntityTable();
        Task<bool> GenarateDbMetadataTable();
    }
}
