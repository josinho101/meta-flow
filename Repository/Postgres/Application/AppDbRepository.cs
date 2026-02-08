using Repository.Admin;
using Repository.Postgres.Admin;

namespace Repository.Postgres.Application
{
    public class AppDbRepository : PostgresDialect, IAppDbRepository
    {
        public AppDbRepository() : base(GetConnectionString())
        {
        }

        public static string GetConnectionString()
        {
            return string.Empty;
        }
    }
}
