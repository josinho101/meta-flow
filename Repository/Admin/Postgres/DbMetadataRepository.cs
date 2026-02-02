using Models;
using Models.Enums;
using Repository.Base;
using System.Data;

namespace Repository.Admin.Postgres
{
    public class DbMetadataRepository : IDbMetadataRepository
    {
        private readonly IDatabaseDialect database;

        private readonly IAppRepository appRepository;

        public DbMetadataRepository(IDatabaseDialect database, IAppRepository appRepository)
        {
            this.database = database;
            this.appRepository = appRepository;
        }

        private async Task CreateDatabase(string dbName, string username, IDbConnection connection)
        {
            string sql = $"CREATE DATABASE {dbName} WITH OWNER = {username}";
            await database.ExecuteNonQueryAsync(connection, sql);
        }

        private async Task CreateDatabaseUser(string username, string password, IDbConnection connection)
        {
            string sql = $"CREATE USER {username} WITH ENCRYPTED PASSWORD '{password}'";
            await database.ExecuteNonQueryAsync(connection, sql);
        }

        private async Task DropDatabaseUser(string username, IDbConnection connection)
        {
            string sql = $"DROP USER IF EXISTS {username}";
            await database.ExecuteNonQueryAsync(connection, sql);
        }

        private async Task DropDatabase(string dbName, IDbConnection connection)
        {
            string sql = $"DROP DATABASE IF EXISTS {dbName} WITH (FORCE)";
            await database.ExecuteNonQueryAsync(connection, sql);
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
            var result = await database.ExecuteNonQueryAsync(connection, sql, parameters);
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
            var result = await database.ExecuteNonQueryAsync(connection, sql, parameters);

            return result > 0;
        }

        public async Task<bool> CreateDb(App app)
        {
            string dbName = $"db_{app.Name}";
            string username = $"user_{app.Name}";
            string password = "somepassword";

            try
            {
                var existingApp = await appRepository.GetByName(app.Name);

                using IDbConnection connection = await database.OpenConnectionAsync();
                await CreateDatabaseUser(username, password, connection);
                await CreateDatabase(dbName, username, connection);
                await SaveDbMetadata(new DbMetadata { AppId = existingApp.Id, DbName = dbName, Username = username, Password = password }, connection);

                return true;
            }
            catch (Exception)
            {
                await DeleteDb(app);
                throw;
            }
        }

        public async Task<bool> DeleteDb(App app)
        {
            string dbName = $"db_{app.Name}";
            string username = $"user_{app.Name}";

            using IDbConnection connection = await database.OpenConnectionAsync();
            await DropDatabase(dbName, connection);
            await DropDatabaseUser(username, connection);
            await DeleteDbMetadata(new DbMetadata { AppId = app.Id }, connection);

            return true;
        }
    }
}
