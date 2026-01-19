using Models.Entity;

namespace Engine.EntityService
{
    public interface IEntityService
    {
        public Task<(Entity? entity, List<string>? errors)> ParseAndValidateAsync(string app, Stream stream);
    }
}
