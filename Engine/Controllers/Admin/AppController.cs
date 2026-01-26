using Microsoft.AspNetCore.Mvc;

namespace Engine.Controllers.Admin
{
    [ApiController]
    [Route("admin/api/apps")]
    public class AppController : Controller
    {
        [HttpPost]
        public IActionResult Create()
        {
            return View();
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
