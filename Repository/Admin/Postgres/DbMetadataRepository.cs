using Repository.Base;
using System.Data;

namespace Repository.Admin.Postgres
{
    public class DbMetadataRepository : IDbMetadataRepository
    {
        private readonly IDatabaseDialect database;

        public DbMetadataRepository(IDatabaseDialect database)
        {
            this.database = database;
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

        private async Task<bool> SaveDbMetadata()
        {
            return await Task.FromResult(true);
        }

        private async Task<bool> DeleteDbMetadata()
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> CreateDb(string appName)
        {
            string dbName = $"db_{appName}";
            string username = $"user_{appName}";
            string password = "somepassword";

            using IDbConnection connection = await database.OpenConnectionAsync();

            try
            {
                await CreateDatabaseUser(username, password, connection);
                await CreateDatabase(dbName, username, connection);

                return true;
            }
            catch (Exception)
            {
                await DropDatabase(dbName, connection);
                await DropDatabaseUser(username, connection);
                throw;
            }
        }

        public async Task<bool> DeleteDb(string appName)
        {
            string dbName = $"db_{appName}";
            string username = $"user_{appName}";

            using IDbConnection connection = await database.OpenConnectionAsync();
           await DropDatabase(dbName, connection);
                await DropDatabaseUser(username, connection);

            return true;
        }
    }
}
