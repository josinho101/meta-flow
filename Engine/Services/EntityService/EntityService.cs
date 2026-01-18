using Engine.EntityService;
using MetaParsers.EntityParser;
using Models.Entity;

namespace Engine.Services.EntityService
{
    public class EntityService : IEntityService
    {
        private readonly IEntityParser<string> parser;
        private readonly ILogger<EntityService> logger;

        public EntityService(IEntityParser<string> parser, ILogger<EntityService> logger)
        {
            this.parser = parser;
            this.logger = logger;
        }

        public async Task<Entity> ParseAndValidateAsync(string app, Stream stream)
        {
            using var reader = new StreamReader(stream);
            string content = await reader.ReadToEndAsync();
            Entity entity = parser.Parse(content);

            // validate entity
            // check if this entity already exists for the app
            // save the entity to meta flow database
            // generate SQL for the entity
            // Apply sql to the app database

            logger.LogInformation($"Parsing completed for entity {entity.Name}, App - {app}");

            return entity;
        }
    }
}
