using Models.Entity;

namespace Repository.Application
{
    public interface IAppEntityRepository
    {
        string GenerateSqlScriptAsync(Entity entity);
        Task<bool> ApplySqlScriptAsync(string sql);
        Task<bool> ApplyDefaultValuesAsync(Entity entity);
    }
}
