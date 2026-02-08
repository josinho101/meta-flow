using Models.Entity;

namespace Engine.Services.AppEntityService
{
    public interface IAppEntityService
    {
        public Task<bool> CreateAppEntityAsync(Entity entity);
    }
}
