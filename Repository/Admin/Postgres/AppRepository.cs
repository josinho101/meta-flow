using Models;
using Models.Enums;
using Repository.Base;
using System.Data;

namespace Repository.Admin.Postgres
{
    public class AppRepository : IAppRepository
    {
        private readonly IDatabaseDialect database;

        public AppRepository(IDatabaseDialect database)
        {
            this.database = database;
        }

        public async Task<App> Create(App app)
        {
            using IDbConnection connection = await database.OpenConnectionAsync();
            DateTime date = DateTime.UtcNow;
            var parameters = new Dictionary<string, object>
            {
                { "name", app.Name },
                { "description", app.Description },
                { "status", (int)Status.Active },
                { "createdDate", date },
                { "updatedDate", date }
            };
            string sql = @"INSERT INTO Apps (Name, Description, Status, CreatedDate, UpdatedDate) 
                                        VALUES (@name, @description, @status, @createdDate, @updatedDate)";
            var result = await database.ExecuteNonQueryAsync(connection, sql, parameters);
            app.Id = result;
            app.CreatedDate = date;
            app.UpdatedDate = date;

            return app;
        }

        public async Task<bool> Delete(int id)
        {
            using IDbConnection connection = await database.OpenConnectionAsync();
            var parameters = new Dictionary<string, object>
            {
                { "id", id },
                { "status", (int)Status.Deleted },
                { "updatedDate", DateTime.UtcNow }
            };
            string sql = @"UPDATE Apps SET Status=@status, UpdatedDate=@updatedDate WHERE Id=@id";
            var result = await database.ExecuteNonQueryAsync(connection, sql, parameters);

            return result > 0;
        }

        public async Task<App> GetById(int id)
        {
            using IDbConnection connection = await database.OpenConnectionAsync();
            var parameters = new Dictionary<string, object>
            {
                { "id", id },
                { "status", (int)Status.Active }
            };
            string sql = @"SELECT Id, Name, Description, CreatedDate FROM Apps WHERE Status=@status AND Id=@id";
            using var reader = await database.ExecuteReaderAsync(connection, sql, parameters);
            while (reader.Read())
            {
                var app = new App
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                };
                return app;
            }

            return null;
        }

        public async Task<App> GetByName(string name)
        {
            using IDbConnection connection = await database.OpenConnectionAsync();
            var parameters = new Dictionary<string, object>
            {
                { "name", name },
                { "status", (int)Status.Active }
            };
            string sql = @"SELECT Id, Name, Description, CreatedDate FROM Apps WHERE Status=@status AND name=@name";
            using var reader = await database.ExecuteReaderAsync(connection, sql, parameters);
            while (reader.Read())
            {
                var app = new App
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                };
                return app;
            }

            return null;
        }

        public async Task<List<App>> GetAll()
        {
            List<App> apps = new List<App>();
            using IDbConnection connection = await database.OpenConnectionAsync();
            var parameters = new Dictionary<string, object>
            {
                { "status", (int)Status.Active }
            };
            string sql = @"SELECT Id, Name, Description, CreatedDate, UpdatedDate FROM Apps WHERE Status=@status";
            using var reader = await database.ExecuteReaderAsync(connection, sql, parameters);
            while (reader.Read())
            {
                var app = new App
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                    UpdatedDate = reader.GetDateTime(reader.GetOrdinal("UpdatedDate"))
                };
                apps.Add(app);
            }

            return apps;
        }

        public async Task<bool> Update(int id, App app)
        {
            using IDbConnection connection = await database.OpenConnectionAsync();
            var parameters = new Dictionary<string, object>
            {
                { "id", id },
                { "name", app.Name },
                { "description", app.Description },
                { "status", (int)Status.Active },
                { "updatedDate", DateTime.UtcNow }
            };
            string sql = @"UPDATE Apps SET Name=@name, Description=@description, UpdatedDate=@updatedDate WHERE Id=@id AND Status=@status";
            var result = await database.ExecuteNonQueryAsync(connection, sql, parameters);

            return result > 0;
        }

        public async Task<bool> FindByName(string name)
        {
            using IDbConnection connection = await database.OpenConnectionAsync();
            var parameters = new Dictionary<string, object>
            {
                { "name", name }
            };
            string sql = @"SELECT COUNT(1) FROM Apps WHERE Name=@name";
            var result = await database.ExecuteScalarAsync(connection, sql, parameters);

            return Convert.ToInt32(result) > 0;
        }

        public async Task<bool> FindById(int id)
        {
            using IDbConnection connection = await database.OpenConnectionAsync();
            var parameters = new Dictionary<string, object>
            {
                { "id", id }
            };
            string sql = @"SELECT COUNT(1) FROM Apps WHERE Id=@id";
            var result = await database.ExecuteScalarAsync(connection, sql, parameters);

            return Convert.ToInt32(result) > 0;
        }
    }
}
