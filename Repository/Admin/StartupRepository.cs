using Microsoft.Extensions.Logging;
using Repository.Base;
using System.Data;

namespace Repository.Admin
{
    public class StartupRepository : IStartupRepository
    {
        private readonly IDatabaseDialect databaseDialect;

        private readonly ILogger<StartupRepository> logger;

        public StartupRepository(IDatabaseDialect databaseDialect, ILogger<StartupRepository> logger)
        {
            this.databaseDialect = databaseDialect;
            this.logger = logger;
        }

        public async Task<bool> GenarateAppTable()
        {
            const string sql = @"
                CREATE TABLE IF NOT EXISTS Apps (
                    id SMALLSERIAL PRIMARY KEY,
                    name VARCHAR(100) UNIQUE,
                    description VARCHAR(500),
                    createdDate TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
                    updatedDate TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
                    status SMALLINT
                );";

            using IDbConnection connection = await databaseDialect.OpenConnectionAsync();
            await databaseDialect.ExecuteNonQueryAsync(connection, sql);
            logger.LogInformation("App table generation completed");
            return true;
        }

        public Task<bool> GenarateEntityTable()
        {
            return Task.FromResult(true);
        }
    }
}