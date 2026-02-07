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

        public async Task<bool> GenarateAppTableAsync()
        {
            const string sql = @"
                CREATE TABLE IF NOT EXISTS Apps (
                    id SMALLSERIAL PRIMARY KEY,
                    name VARCHAR(100) UNIQUE NOT NULL,
                    description VARCHAR(500),
                    createdDate TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
                    updatedDate TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
                    status SMALLINT NOT NULL
                );";

            using IDbConnection connection = await database.OpenConnectionAsync();
            await database.ExecuteNonQueryAsync(connection, sql);
            return true;
        }

        public async Task<bool> GenarateDbMetadataTableAsync()
        {
            const string sql = @"
                CREATE TABLE IF NOT EXISTS DbMetadata (
                    id SMALLSERIAL PRIMARY KEY,
                    appId INTEGER NOT NULL,
                    name VARCHAR(100) NOT NULL UNIQUE,
                    username VARCHAR(50) NOT NULL,
                    password VARCHAR(100) NOT NULL,
                    createdDate TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
                    updatedDate TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
                    status SMALLINT NOT NULL,

                    CONSTRAINT fk_App
                      FOREIGN KEY(appId) 
                      REFERENCES Apps(id)
                      ON DELETE CASCADE
                );";

            using IDbConnection connection = await database.OpenConnectionAsync();
            await database.ExecuteNonQueryAsync(connection, sql);
            return true;
        }

        public async Task<bool> GenarateEntityTableAsync()
        {
            const string sql = @"
                CREATE TABLE IF NOT EXISTS Entities (
                    id SMALLSERIAL PRIMARY KEY,
                    appId INTEGER NOT NULL,
                    name VARCHAR(50) NOT NULL,
                    metadata JSONB NOT NULL,
                    createdDate TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
                    updatedDate TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
                    status SMALLINT NOT NULL,

                    CONSTRAINT fk_App
                      FOREIGN KEY(appId) 
                      REFERENCES Apps(id)
                      ON DELETE CASCADE,

                   CONSTRAINT uq_app_id_name UNIQUE (appId, name)
                );";

            using IDbConnection connection = await database.OpenConnectionAsync();
            await database.ExecuteNonQueryAsync(connection, sql);
            return true;
        }
    }
}