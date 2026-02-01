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

        private async Task CreateDatabase(string dbName, string username)
        {
            using IDbConnection connection = await database.OpenConnectionAsync();
            string sql = $"CREATE DATABASE {dbName} WITH OWNER = {username}";
            await database.ExecuteNonQueryAsync(connection, sql);
        }
        private async Task CreateDatabaseUser(string username, string password)
        {
            using IDbConnection connection = await database.OpenConnectionAsync();
            string sql = $"CREATE USER {username} WITH ENCRYPTED PASSWORD '{password}'";
            await database.ExecuteNonQueryAsync(connection, sql);
        }

        private async Task DropDatabaseUser(string username)
        {
            using IDbConnection connection = await database.OpenConnectionAsync();
            string sql = $"DROP USER IF EXISTS {username}";
            await database.ExecuteNonQueryAsync(connection, sql);
        }

        private async Task DropDatabase(string dbName)
        {
            using IDbConnection connection = await database.OpenConnectionAsync();
            string sql = $"DROP DATABASE IF EXISTS {dbName} WITH (FORCE)";
            await database.ExecuteNonQueryAsync(connection, sql);
        }

        public async Task<bool> CreateDb(string appName)
        {
            string databaseName = $"db_{appName}";
            string username = $"user_{appName}";
            string password = "somepassword";

            try
            {
                await CreateDatabaseUser(username, password);
                await CreateDatabase(databaseName, username);

                return true;
            }
            catch (Exception)
            {
                await DropDatabase(databaseName);
                await DropDatabaseUser(username);
                throw;
            }
        }

        public async Task<bool> DeleteDb(string appName)
        {
            string databaseName = $"db_{appName}";
            string username = $"user_{appName}";

            await DropDatabase(databaseName);
            await DropDatabaseUser(username);

            return true;
        }
    }
}
