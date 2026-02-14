using Microsoft.AspNetCore.Mvc;
using Models.Entity;

namespace Engine.Controllers.Admin
{
    [ApiController]
    [Route("admin/api/entity-fieldtypes")]
    public class AppEntityFieldTypeController : Controller
    {
        private readonly IConfiguration config;

        public AppEntityFieldTypeController(IConfiguration config)
        {
            this.config = config;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var types = config.GetSection("EntityFieldTypes").Get<List<FieldTypeInfo>>();
            return Ok(types);
        }
    }
}
