using Models.Entity;

namespace Validators.EntityValidator
{
    public interface IEntityValidator
    {
        public bool Validate(Entity entity);
    }
}
