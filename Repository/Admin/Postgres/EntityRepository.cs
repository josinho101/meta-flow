using Models.Entity;
using Models.Enums;
using Repository.Base;
using System.Data;
using System.Text.Json;

namespace Repository.Admin.Postgres
{
    public class EntityRepository : IEntityRepository
    {
        private readonly IDatabaseDialect database;

        public EntityRepository(IDatabaseDialect database)
        {
            this.database = database;
        }

        public Task<Entity> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Entity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, Entity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Entity> SaveAsync(Entity entity)
        {
            using IDbConnection connection = await database.OpenConnectionAsync();
            DateTime date = DateTime.UtcNow;
            var parameters = new Dictionary<string, object>
            {
                { "appId", entity.AppId },
                { "name", entity.Name },
                { "metadata", JsonSerializer.Serialize(entity) },
                { "createdDate", date },
                { "updatedDate", date },
                { "status", (int)Status.Active }
            };
            string sql = @"INSERT INTO Entities (appId, name, metadata, createdDate, updatedDate, status) 
                                        VALUES (@appId, @name, @metadata::jsonb, @createdDate, @updatedDate, @status)";
            var result = await database.ExecuteNonQueryAsync(connection, sql, parameters);

            return entity;
        }
    }
}
