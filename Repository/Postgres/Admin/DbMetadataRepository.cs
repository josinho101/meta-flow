using Microsoft.Extensions.Logging;
using Models;
using Models.Enums;
using Repository.Admin;
using System.Data;

namespace Repository.Postgres.Admin
{
    public class DbMetadataRepository : IDbMetadataRepository
    {
        private readonly IMetaFlowRepository repository;

        private readonly ILogger<DbMetadataRepository> logger;

        public DbMetadataRepository(IMetaFlowRepository repository, ILogger<DbMetadataRepository> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        private async Task CreateDatabaseAsync(string dbName, string username, IDbConnection connection)
        {
            string sql = $"CREATE DATABASE {dbName} WITH OWNER = {username}";
            await repository.ExecuteNonQueryAsync(connection, sql);
        }

        private async Task CreateDatabaseUserAsync(string username, string password, IDbConnection connection)
        {
            string sql = $"CREATE USER {username} WITH ENCRYPTED PASSWORD '{password}'";
            await repository.ExecuteNonQueryAsync(connection, sql);
        }

        private async Task DropDatabaseUserAsync(string username, IDbConnection connection)
        {
            try
            {
                string sql = $"DROP USER IF EXISTS {username}";
                await repository.ExecuteNonQueryAsync(connection, sql);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to drop database user {username}");
            }
        }

        private async Task DropDatabaseAsync(string dbName, IDbConnection connection)
        {
            try
            {
                string sql = $"DROP DATABASE IF EXISTS {dbName} WITH (FORCE)";
                await repository.ExecuteNonQueryAsync(connection, sql);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to drop database {dbName}");
            }
        }

        private async Task<bool> SaveDbMetadata(DbMetadata metadata, IDbConnection connection)
        {
            DateTime date = DateTime.UtcNow;
            var parameters = new Dictionary<string, object>
            {
                { "appId", metadata.AppId },
                { "name", metadata.DbName },
                { "username", metadata.Username },
                { "password", metadata.Password },
                { "status", (int)Status.Active },
                { "createdDate", date },
                { "updatedDate", date }
            };
            string sql = @"INSERT INTO DbMetadata (appId, name, username, password, createdDate, updatedDate, status) 
                              VALUES (@appId, @name, @username, @password, @createdDate, @updatedDate, @status)";
            var result = await repository.ExecuteNonQueryAsync(connection, sql, parameters);
            return true;
        }

        private async Task<bool> DeleteDbMetadata(DbMetadata metadata, IDbConnection connection)
        {
            var parameters = new Dictionary<string, object>
            {
                { "appId", metadata.AppId },
                { "status", (int)Status.Deleted },
                { "updatedDate", DateTime.UtcNow }
            };
            string sql = @"UPDATE DbMetadata SET Status=@status, UpdatedDate=@updatedDate WHERE appId=@appId";
            var result = await repository.ExecuteNonQueryAsync(connection, sql, parameters);

            return result > 0;
        }

        public async Task<bool> CreateDbAsync(int appId, DbMetadata dbMetadata)
        {
            string dbName = dbMetadata.DbName;
            string username = dbMetadata.Username;
            string password = dbMetadata.Password;

            using IDbConnection connection = await repository.OpenConnectionAsync();
            await CreateDatabaseUserAsync(username, password, connection);
            await CreateDatabaseAsync(dbName, username, connection);
            await SaveDbMetadata(new DbMetadata { AppId = appId, DbName = dbName, Username = username, Password = password }, connection);

            return true;
        }

        public async Task<bool> DeleteDbAsync(int appId)
        {

            var metadata = await this.GetDbMetadataByAppNameAsync(appId);
            if (metadata == null)
            {
                throw new Exception($"DbMetadata for app id {appId} not found.");
            }

            string dbName = metadata.DbName;
            string username = metadata.Username;

            using IDbConnection connection = await repository.OpenConnectionAsync();
            await DropDatabaseAsync(dbName, connection);
            await DropDatabaseUserAsync(username, connection);
            await DeleteDbMetadata(new DbMetadata { AppId = metadata.AppId }, connection);

            return true;
        }

        public async Task<DbMetadata> GetDbMetadataByAppNameAsync(int appId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "appId", appId },
                { "status", (int)Status.Active }
            };
            string sql = "SELECT name, username, password FROM DbMetadata WHERE appId = @appId AND status = @status";

            using IDbConnection connection = await repository.OpenConnectionAsync();
            using var reader = await repository.ExecuteReaderAsync(connection, sql, parameters);
            DbMetadata dbMetadata = null;
            if (reader.Read())
            {
                dbMetadata = new DbMetadata
                {
                    AppId = appId,
                    DbName = reader.GetString(reader.GetOrdinal("name")),
                    Username = reader.GetString(reader.GetOrdinal("username")),
                    Password = reader.GetString(reader.GetOrdinal("password"))
                };
            }

            return dbMetadata;
        }

        public async Task<bool> IsValidDbNameAsync(string dbName)
        {
            var parameters = new Dictionary<string, object>
            {
                { "dbName", dbName }
            };
            const string sql = "SELECT 1 FROM pg_database WHERE datname = @dbName";
            using IDbConnection connection = await repository.OpenConnectionAsync();
            var exists = await repository.ExecuteScalarAsync(connection, sql, parameters);
            bool dbFound = exists != null && Convert.ToInt32(exists) > 0;
            return !dbFound;
        }

        public async Task<bool> IsValidUsernameAsync(string username)
        {
            var parameters = new Dictionary<string, object>
            {
                { "username", username }
            };
            const string sql = "SELECT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = @username)";
            using IDbConnection connection = await repository.OpenConnectionAsync();
            var exists = await repository.ExecuteScalarAsync(connection, sql, parameters);
            bool userFound = exists != null && Convert.ToInt32(exists) > 0;

            return !userFound;
        }
    }
}
