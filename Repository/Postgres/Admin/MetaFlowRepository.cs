using Microsoft.Extensions.Configuration;
using Repository.Admin;

namespace Repository.Postgres.Admin
{
    public class MetaFlowRepository : PostgresDialect, IMetaFlowRepository
    {
        public MetaFlowRepository(IConfiguration configuration) : base(GetConnectionString(configuration))
        {
        }

        public static string GetConnectionString(IConfiguration configuration) {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Connection string 'DefaultConnection' is not found or is empty.");
            }
            return connectionString;
        }
    }
}
