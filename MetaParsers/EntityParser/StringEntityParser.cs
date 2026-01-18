using Models.Entity;
using System.Text.Json;

namespace MetaParsers.EntityParser
{
    public class StringEntityParser : IEntityParser<string>
    {
        JsonSerializerOptions options = new JsonSerializerOptions{ 
            PropertyNameCaseInsensitive = true, 
            AllowTrailingCommas = true 
        };

        public Entity Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input is null or empty.", nameof(input));
            }

            Entity entity;
            try
            {
                entity = JsonSerializer.Deserialize<Entity>(input, options);
            }
            catch (JsonException ex)
            {
                throw new JsonException("Failed to deserialize input", ex);
            }

            if (entity == null)
            {
                throw new JsonException("Deserialized Entity is null.");
            }

            return entity;
        }
    }
}
