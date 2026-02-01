using System.Data;
using Repository.Base;

namespace Repository.Admin.Postgres
{
    public class StartupRepository : IStartupRepository
    {
        private readonly IDatabaseDialect database;

        public StartupRepository(IDatabaseDialect database)
        {
            this.database = database;
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

            using IDbConnection connection = await database.OpenConnectionAsync();
            await database.ExecuteNonQueryAsync(connection, sql);
            return true;
        }

        public async Task<bool> GenarateDbMetadataTable()
        {
            const string sql = @"
                CREATE TABLE IF NOT EXISTS DbMetadata (
                    id SMALLSERIAL PRIMARY KEY,
                    appId INTEGER,
                    name VARCHAR(100) UNIQUE,
                    username VARCHAR(50),
                    password VARCHAR(100),
                    createdDate TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
                    updatedDate TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
                    status SMALLINT,

                    CONSTRAINT fk_App
                      FOREIGN KEY(appId) 
                      REFERENCES Apps(id)
                      ON DELETE CASCADE
                );";

            using IDbConnection connection = await database.OpenConnectionAsync();
            await database.ExecuteNonQueryAsync(connection, sql);
            return true;
        }

        public Task<bool> GenarateEntityTable()
        {
            return Task.FromResult(true);
        }
    }
}