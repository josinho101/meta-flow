using Models.Entity;

namespace Repository.Application
{
    public interface IAppEntityRepository
    {
        Task<string> GenerateSqlScriptAsync(Entity entity);
        Task<bool> ApplySqlScriptAsync(string sql);
    }
}
