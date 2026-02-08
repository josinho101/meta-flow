using Models;
using Models.Enums;
using Repository.Admin;
using System.Data;

namespace Repository.Postgres.Admin
{
    public class DbMetadataRepository : IDbMetadataRepository
    {
        private readonly IMetaFlowRepository repository;

        private readonly IAppRepository appRepository;

        public DbMetadataRepository(IMetaFlowRepository repository, IAppRepository appRepository)
        {
            this.repository = repository;
            this.appRepository = appRepository;
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
            string sql = $"DROP USER IF EXISTS {username}";
            await repository.ExecuteNonQueryAsync(connection, sql);
        }

        private async Task DropDatabaseAsync(string dbName, IDbConnection connection)
        {
            string sql = $"DROP DATABASE IF EXISTS {dbName} WITH (FORCE)";
            await repository.ExecuteNonQueryAsync(connection, sql);
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
            return await Task.FromResult(true);
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

        public async Task<bool> CreateDbAsync(App app)
        {
            string dbName = $"db_{app.Name}";
            string username = $"user_{app.Name}";
            string password = "somepassword";

            try
            {
                var existingApp = await appRepository.GetByNameAsync(app.Name);

                using IDbConnection connection = await repository.OpenConnectionAsync();
                await CreateDatabaseUserAsync(username, password, connection);
                await CreateDatabaseAsync(dbName, username, connection);
                await SaveDbMetadata(new DbMetadata { AppId = existingApp.Id, DbName = dbName, Username = username, Password = password }, connection);

                return true;
            }
            catch (Exception)
            {
                await DeleteDbAsync(app);
                throw;
            }
        }

        public async Task<bool> DeleteDbAsync(App app)
        {
            string dbName = $"db_{app.Name}";
            string username = $"user_{app.Name}";

            using IDbConnection connection = await repository.OpenConnectionAsync();
            await DropDatabaseAsync(dbName, connection);
            await DropDatabaseUserAsync(username, connection);
            await DeleteDbMetadata(new DbMetadata { AppId = app.Id }, connection);

            return true;
        }

        public async Task<DbMetadata> GetDbMetadataByAppNameAsync(string appName)
        {
            var app = await appRepository.GetByNameAsync(appName);
            if (app == null)
            {
                throw new Exception($"App with name {appName} not found.");
            }

            var parameters = new Dictionary<string, object>
            {
                { "appId", app.Id },
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
                    AppId = app.Id,
                    DbName = reader.GetString(reader.GetOrdinal("name")),
                    Username = reader.GetString(reader.GetOrdinal("username")),
                    Password = reader.GetString(reader.GetOrdinal("password"))
                };
            }

            return dbMetadata;
        }
    }
}
