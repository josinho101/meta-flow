namespace Repository.Admin
{
    public interface IMetaFlowRepository
    {
        Task<bool> GenarateAppTable();
        Task<bool> GenarateEntityTable();
    }
}
