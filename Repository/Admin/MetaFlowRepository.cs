using Microsoft.Extensions.Logging;
using Repository.Base;
using System.Data;

namespace Repository.Admin
{
    public class MetaFlowRepository : IMetaFlowRepository
    {
        private readonly IDatabaseDialect databaseDialect;

        private readonly ILogger<MetaFlowRepository> logger;

        public MetaFlowRepository(IDatabaseDialect databaseDialect, ILogger<MetaFlowRepository> logger)
        {
            this.databaseDialect = databaseDialect;
            this.logger = logger;
        }

        public async Task<bool> GenarateAppTable()
        {
            const string sql = @"
                CREATE TABLE IF NOT EXISTS Apps (
                    id SERIAL PRIMARY KEY,
                    name VARCHAR(100),
                    description VARCHAR(500),
                    createdDate TIMESTAMP DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
                    modifiedDate TIMESTAMP DEFAULT (CURRENT_TIMESTAMP AT TIME ZONE 'UTC'),
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