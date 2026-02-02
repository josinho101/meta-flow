using Models.Entity;

namespace Repository.Admin
{
    public interface IEntityRepository
    {
        public Task<Entity> SaveAsync(Entity entity);

        public Task<Entity> GetAsync();

        public Task<List<Entity>> GetAllAsync();

        public Task<bool> UpdateAsync(int id, Entity entity);

        public Task<bool> DeleteAsync(int id);
    }
}
