using Engine.Models;
using MetaParsers.EntityParser;
using Microsoft.AspNetCore.Mvc;
using Models.Entity;
using System.Net;
using System.Net.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Engine.Controllers.Admin
{
    [ApiController]
    [Route("admin/api/{app}/entity")]
    public class EntityController : Controller
    {
        private readonly IEntityParser<string> parser;
        private readonly ILogger<EntityController> logger;

        public EntityController(IEntityParser<string> parser, ILogger<EntityController> logger)
        {
            this.parser = parser;
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
                using var reader = new StreamReader(stream);
                string content = await reader.ReadToEndAsync();
                entity = parser.Parse(content);

                logger.LogInformation($"Parsing completed for entity {entity.Name}, App - {app}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while parsing entity file - {FileName}", file.FileName);
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    new ErrorResponse("Error while parsing file - {file?.FileName}"));
            }

            return Created(string.Empty, new SucessResponse<object>() { 
                Success = true, 
                Message = $"Entity {entity.Name} created sucessfully." });
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
