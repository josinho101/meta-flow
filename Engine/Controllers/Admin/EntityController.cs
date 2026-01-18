using Engine.EntityService;
using Engine.Models;
using Microsoft.AspNetCore.Mvc;
using Models.Entity;
using System.Net;

namespace Engine.Controllers.Admin
{
    [ApiController]
    [Route("admin/api/{app}/entity")]
    public class EntityController : Controller
    {
        private readonly IEntityService entityService;
        private readonly ILogger<EntityController> logger;

        public EntityController(IEntityService entityService, ILogger<EntityController> logger)
        {
            this.entityService = entityService;
            this.logger = logger;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(string app, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                logger.LogError("Multipart file not found in request. App - {app}", app);
                return BadRequest(new ErrorResponse("No entity file found. Use field name 'file' to upload entity"));
            }

            Entity entity;
            try
            {
                using var stream = file.OpenReadStream();
                entity = await entityService.ParseAndValidateAsync(app, stream);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while parsing entity file - {FileName}", file.FileName);
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    new ErrorResponse("Error while parsing file - {file?.FileName}"));
            }

            return Created(string.Empty, new SucessResponse<object>() { 
                Success = true, 
                Message = $"Entity {entity.Name} created sucessfully." 
            });
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
