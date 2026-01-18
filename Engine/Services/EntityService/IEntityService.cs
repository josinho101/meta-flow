using Models.Entity;

namespace Engine.EntityService
{
    public interface IEntityService
    {
        public Task<Entity> ParseAndValidateAsync(string app, Stream stream);
    }
}
