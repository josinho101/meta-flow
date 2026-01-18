using Models.Entity;

namespace Validators.EntityValidator
{
    public class EntityValidator : IEntityValidator
    {
        public bool Validate(Entity entity)
        {
            if (entity is null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                return false;
            }

            if (entity.Fields is null || !entity.Fields.Any())
            {
                return false;
            }

            foreach (var field in entity.Fields)
            {
                if (field is null)
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(field.Field))
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(field.Type))
                {
                    return false;
                }

                var fk = field.ForeignKey;
                if (fk is not null)
                {
                    if (string.IsNullOrWhiteSpace(fk.ReferenceTable) || string.IsNullOrWhiteSpace(fk.ReferenceField))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
