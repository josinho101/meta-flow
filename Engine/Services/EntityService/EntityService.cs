using Engine.EntityService;
using Engine.Exceptions;
using MetaParsers.EntityParser;
using Models.Entity;
using Repository.Admin;
using Validators.EntityValidator;

namespace Engine.Services.EntityService
{
    public class EntityService : IEntityService
    {
        private readonly IEntityParser<string> parser;
        private readonly IEntityValidator entityValidator;
        private readonly ILogger<EntityService> logger;
        private readonly IEntityRepository entityRepository;
        private readonly IAppRepository appRepository;

        public EntityService(
            IEntityParser<string> parser, 
            ILogger<EntityService> logger, 
            IEntityValidator entityValidator,
            IEntityRepository entityRepository,
            IAppRepository appRepository)
        {
            this.parser = parser;
            this.logger = logger;
            this.entityValidator = entityValidator;
            this.entityRepository = entityRepository;
            this.appRepository = appRepository;
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

        public async Task<bool> SaveAsync(string appName, Entity entity)
        {
            try
            {
                var app = await appRepository.GetByNameAsync(appName);
                if (app == null)
                {
                    logger.LogError($"App {appName} not found while creating entity {entity.Name}");
                    throw new EntityNotFoundException($"App {appName} not found while creating entity {entity.Name}");
                }
                entity.AppId = app.Id;
                await entityRepository.SaveAsync(entity);
                logger.LogInformation($"Entity {entity.Name} saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error saving entity {entity.Name}: {ex.Message}");
                return false;
            }
        }
    }
}
