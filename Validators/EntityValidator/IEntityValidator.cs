using Models.Entity;

namespace Validators.EntityValidator
{
    public interface IEntityValidator
    {
        public List<string> Validate(Entity entity);
    }
}
