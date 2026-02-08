using Models.Entity;
using Repository.Admin;
using Repository.Application;

namespace Repository.Postgres.Application
{
    public class AppEntityRepository : IAppEntityRepository
    {
        private readonly IAppDbRepository appDbRepository;

        public AppEntityRepository(IAppDbRepository appDbRepository)
        {
            this.appDbRepository = appDbRepository;
        }

        public Task<bool> ApplySqlScriptAsync(string sql)
        {
            return Task.FromResult(true);
        }

        public Task<string> GenerateSqlScriptAsync(Entity entity)
        {
            return Task.FromResult(string.Empty);
        }
    }
}
