using Engine.Models.ViewModels;
using Engine.Services.AppsService;
using Microsoft.AspNetCore.Mvc;
using Models.Apps;

namespace Engine.Controllers.Admin
{
    [ApiController]
    [Route("admin/api/apps")]
    public class AppController : Controller
    {
        private readonly IAppService appsService;

        public AppController(IAppService appsService)
        {
            this.appsService = appsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppViewModel app)
        {
            var newApp = await appsService.Create(app);
            return Created($"/apps/{newApp.Id}", newApp);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var apps = await appsService.GetAll();
            return Ok(apps);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var app = await appsService.Get(id);
            if(app == null)
            {
                return NotFound();
            }
            return Ok(app);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AppViewModel app)
        {
            var updatedApp = await appsService.Update(id, app);
            return Ok(updatedApp);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await appsService.Delete(id);
            return Ok(result);
        }
    }
}
