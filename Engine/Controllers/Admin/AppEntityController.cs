using Engine.EntityService;
using Engine.Models;
using Engine.Services.AppEntityService;
using Microsoft.AspNetCore.Mvc;
using Repository.Admin;

namespace Engine.Controllers.Admin
{
    [ApiController]
    [Route("admin/api/{appName}/entity")]
    public class AppEntityController : Controller
    {
        private readonly IEntityService entityService;

        private readonly ILogger<AppEntityController> logger;

        private readonly IAppEntityService appEntityService;

        public AppEntityController(
            IEntityService entityService, 
            ILogger<AppEntityController> logger, 
            IAppDbRepository appDbRepository,
            IAppEntityService appEntityService)
        {
            this.logger = logger;
            this.entityService = entityService;
            this.appEntityService = appEntityService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(string appName, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                logger.LogError("Multipart file not found in request. App - {app}", appName);
                return BadRequest(new ErrorResponse("No entity file found. Use field name 'file' to upload entity"));
            }

            using var stream = file.OpenReadStream();
            var (entity, errors) = await entityService.ParseAndValidateAsync(appName, stream);

            if (errors != null && errors.Any())
            {
                var errorSummary = string.Join(", ", errors);
                return BadRequest(new ErrorResponse($"Entity validation for {appName} failed with following errors. {errorSummary}"));
            }
            else
            {
                await entityService.SaveAsync(appName, entity);
                await appEntityService.CreateAppEntityAsync(entity);
                return Created(string.Empty, new SucessResponse<object>()
                {
                    Success = true,
                    Message = $"Entity {entity?.Name} created sucessfully."
                });
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult Update()
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return Ok();
        }
    }
}
