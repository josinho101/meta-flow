using Models.Enums;
using Repository.Admin;
using System.Data;

namespace Repository.Postgres.Admin
{
    public class StartupRepository : IStartupRepository
    {
        private readonly IMetaFlowRepository repository;

        public StartupRepository(IMetaFlowRepository repository)
        {
            this.repository = repository;
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

            using IDbConnection connection = await repository.OpenConnectionAsync();
            await repository.ExecuteNonQueryAsync(connection, sql);
            return true;
        }

        public async Task<bool> GenarateDbMetadataTableAsync()
        {
            string sql = $@"
                CREATE TABLE IF NOT EXISTS DbMetadata (
                    id SMALLSERIAL PRIMARY KEY,
                    appId INTEGER NOT NULL,
                    name VARCHAR(50) NOT NULL,
                    username VARCHAR(50) NOT NULL,
                    password VARCHAR(100) NOT NULL,
                    createdDate TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
                    updatedDate TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
                    status SMALLINT NOT NULL,

                    CONSTRAINT fk_App
                      FOREIGN KEY(appId) 
                      REFERENCES Apps(id)
                      ON DELETE CASCADE
                );
                CREATE UNIQUE INDEX IF NOT EXISTS idx_dbmetadata_name_active 
                ON DbMetadata (name) 
                WHERE status != {(int)Status.Deleted};";

            using IDbConnection connection = await repository.OpenConnectionAsync();
            await repository.ExecuteNonQueryAsync(connection, sql);
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

                   CONSTRAINT uq_app_id_name_status UNIQUE (appId, name, status)
                );";

            using IDbConnection connection = await repository.OpenConnectionAsync();
            await repository.ExecuteNonQueryAsync(connection, sql);
            return true;
        }
    }
}