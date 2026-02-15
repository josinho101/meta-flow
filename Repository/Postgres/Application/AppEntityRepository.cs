using Models.Entity;
using Repository.Admin;
using Repository.Application;
using System.Data;
using System.Text;

namespace Repository.Postgres.Application
{
    public class AppEntityRepository : IAppEntityRepository
    {
        private readonly IAppDbRepository appDbRepository;

        public AppEntityRepository(IAppDbRepository appDbRepository)
        {
            this.appDbRepository = appDbRepository;
        }

        public async Task<bool> ApplySqlScriptAsync(string sql)
        {
            using IDbConnection connection = await appDbRepository.OpenConnectionAsync();
            var result = await appDbRepository.ExecuteNonQueryAsync(connection, sql);
            return true;
        }

        public string GenerateSqlScriptAsync(Entity entity)
        {
            AddDefaultColumns(entity);

            var sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE {entity.Name} (");

            var columnDefinitions = new List<string>();
            var foreignKeyConstraints = new List<string>();

            foreach (var field in entity.Fields)
            {
                var columnParts = new List<string> { field.Field };

                // 1. Handle Type / Auto-generation
                if (field.PrimaryKey != null && field.PrimaryKey.AutoGenerate)
                {
                    columnParts.Add("SERIAL");
                }
                else
                {
                    columnParts.Add(MapType(field.Type, field.Length));
                }

                // 2. Handle Default Value Logic
                if (!string.IsNullOrWhiteSpace(field.DefaultValue))
                {
                    columnParts.Add($"DEFAULT {field.DefaultValue}");
                }

                // 3. Handle Constraints
                if (field.PrimaryKey != null)
                {
                    columnParts.Add("PRIMARY KEY");
                }
                else
                {
                    if (!field.Nullable) columnParts.Add("NOT NULL");
                    if (field.Unique) columnParts.Add("UNIQUE");
                }

                columnDefinitions.Add("    " + string.Join(" ", columnParts));

                if (field.ForeignKey != null)
                {
                    foreignKeyConstraints.Add($"    FOREIGN KEY ({field.Field}) REFERENCES {field.ForeignKey.ReferenceTable}({field.ForeignKey.ReferenceField})");
                }
            }

            sb.Append(string.Join(",\n", columnDefinitions.Concat(foreignKeyConstraints)));
            sb.AppendLine("\n);");

            return sb.ToString();
        }

        private void AddDefaultColumns(Entity entity)
        {
            var defaultFields = new List<EntityField>
            {
                new EntityField {
                    Field = "createdDate",
                    Type = "timestamptz",
                    Nullable = false,
                    DefaultValue = "CURRENT_TIMESTAMP"
                },
                new EntityField {
                    Field = "updatedDate",
                    Type = "timestamptz",
                    Nullable = false,
                    DefaultValue = "CURRENT_TIMESTAMP"
                },
                new EntityField {
                    Field = "status",
                    Type = "smallint",
                    Nullable = false
                }
            };

            // Add only if the field doesn't already exist to avoid duplicates
            foreach (var df in defaultFields)
            {
                if (!entity.Fields.Any(f => f.Field.Equals(df.Field, StringComparison.OrdinalIgnoreCase)))
                {
                    entity.Fields.Add(df);
                }
            }
        }

        private string MapType(string type, int length)
        {
            return type.ToLower().Trim() switch
            {
                "smallint" => "SMALLINT",
                "integer" => "INTEGER",
                "bigint" => "BIGINT",
                "boolean" => "BOOLEAN",
                "timestamptz" => "TIMESTAMPTZ",

                // Exact numeric with precision. 
                // Defaulting to (18,2) if no length is specified is common practice.
                "decimal" => length > 0 ? $"DECIMAL({length},2)" : "DECIMAL(18,2)",

                // Variable-length character string
                "varchar" => length > 0 ? $"VARCHAR({length})" : "TEXT",

                // Fallback for types not explicitly listed or already in SQL format
                _ => type.ToUpper()
            };
        }

        public async Task<bool> ApplyDefaultValuesAsync(Entity entity)
        {
            return await Task.FromResult(true);
        }
    }
}
