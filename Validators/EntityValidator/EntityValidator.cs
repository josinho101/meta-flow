using Microsoft.Extensions.Configuration;
using Models.Entity;

namespace Validators.EntityValidator
{
    public class EntityValidator : IEntityValidator
    {
        private readonly HashSet<string> allowedTypes;

        public EntityValidator(IConfiguration configuration)
        {
            var types = configuration.GetSection("EntityFieldTypes").Get<List<FieldTypeInfo>>();
            allowedTypes = new HashSet<string>(
                types
                    .Where(t => !string.IsNullOrWhiteSpace(t.Type))
                    .Select(t => t.Type.Trim()),
                StringComparer.OrdinalIgnoreCase);
        }

        public List<string> Validate(Entity entity)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                errors.Add("Entity name is required.");
            }

            if (entity.Fields is null || !entity.Fields.Any())
            {
                errors.Add("Entity must contain at least one field.");
            }
            else
            {
                for (int i = 0; i < entity.Fields.Count; i++)
                {
                    var field = entity.Fields[i];
                    var fieldId = field?.Field ?? $"index {i}";

                    if (string.IsNullOrWhiteSpace(field.Field))
                    {
                        errors.Add($"Field name is required for field at index {i}");
                    }

                    if (string.IsNullOrWhiteSpace(field.Type))
                    {
                        errors.Add($"Type is required for field '{fieldId}'");
                    }
                    else
                    {
                        var fieldType = field.Type.Trim();
                        if (!allowedTypes.Contains(fieldType))
                        {
                            errors.Add($"Field '{fieldId}' has unsupported type '{fieldType}'");
                        }
                    }

                    var fk = field.ForeignKey;
                    if (fk is not null)
                    {
                        if (string.IsNullOrWhiteSpace(fk.ReferenceTable))
                        {
                            errors.Add($"ForeignKey on field '{fieldId}' is missing ReferenceTable");
                        }

                        if (string.IsNullOrWhiteSpace(fk.ReferenceField))
                        {
                            errors.Add($"ForeignKey on field '{fieldId}' is missing ReferenceField");
                        }
                    }
                }
            }

            return errors;
        }
    }
}
