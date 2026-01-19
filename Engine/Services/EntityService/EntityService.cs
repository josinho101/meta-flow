using Engine.EntityService;
using MetaParsers.EntityParser;
using Models.Entity;
using Validators.EntityValidator;

namespace Engine.Services.EntityService
{
    public class EntityService : IEntityService
    {
        private readonly IEntityParser<string> parser;
        private readonly IEntityValidator entityValidator;
        private readonly ILogger<EntityService> logger;

        public EntityService(
            IEntityParser<string> parser, 
            ILogger<EntityService> logger, 
            IEntityValidator entityValidator)
        {
            this.parser = parser;
            this.logger = logger;
            this.entityValidator = entityValidator;
        }

        public async Task<(Entity? entity, List<string>? errors)> ParseAndValidateAsync(string app, Stream stream)
        {
            using var reader = new StreamReader(stream);
            string content = await reader.ReadToEndAsync();
            Entity entity = parser.Parse(content);
            var errors = entityValidator.Validate(entity);

            if (errors.Any())
            {
                var errorSummary = string.Join(", ", errors);
                logger.LogError($"Entity validation for {app} failed with following errors. {errorSummary}");
                return (null, errors);
            }

            // validate entity
            // check if this entity already exists for the app
            // save the entity to meta flow database
            // generate SQL for the entity
            // Apply sql to the app database

            logger.LogInformation($"Parsing completed for entity {entity.Name}, App - {app}");

            return (entity, null);
        }
    }
}
