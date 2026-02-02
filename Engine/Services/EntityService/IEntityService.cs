using Models.Entity;

namespace Engine.EntityService
{
    public interface IEntityService
    {
        public Task<(Entity? entity, List<string>? errors)> ParseAndValidateAsync(string app, Stream stream);

        public Task<bool> SaveAsync(string appName, Entity entity);
    }
}
