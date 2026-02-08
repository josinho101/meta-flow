using Models;
using Repository.Admin;
using Repository.Postgres.Admin;
using Microsoft.Extensions.Configuration;

namespace Repository.Postgres.Application
{
    public class AppDbRepository : PostgresDialect, IAppDbRepository
    {
        public AppDbRepository(DbMetadata dbMetadata, IConfiguration configuration) : 
            base(GetConnectionString(dbMetadata, configuration))
        {
        }

        public static string GetConnectionString(DbMetadata dbMetadata, IConfiguration configuration)
        {
            string host = configuration["AppDatabase:Host"] ?? "localhost";
            string port = configuration["AppDatabase:Port"] ?? "5432";
            string username = dbMetadata.Username;
            string password = dbMetadata.Password;
            string dbName = dbMetadata.DbName;

            return $"Host={host};Port={port};Database={dbName};Username={username};Password={password}";
        }
    }
}
