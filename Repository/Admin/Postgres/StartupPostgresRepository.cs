using System.Data;
using Repository.Base;

namespace Repository.Admin.Postgres
{
    public class StartupPostgresRepository : IStartupRepository
    {
        private readonly IDatabaseDialect databaseDialect;

        public StartupPostgresRepository(IDatabaseDialect databaseDialect)
        {
            this.databaseDialect = databaseDialect;
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
            return true;
        }

        public Task<bool> GenarateEntityTable()
        {
            return Task.FromResult(true);
        }
    }
}