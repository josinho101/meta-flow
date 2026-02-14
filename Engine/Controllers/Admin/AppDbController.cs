using Engine.Models.ViewModels;
using Engine.Services.AppDb;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Controllers.Admin
{
    [ApiController]
    [Route("admin/api")]
    public class AppDbController : Controller
    {
        private readonly IAppDbService appDbService;

        private readonly ILogger<AppDbController> logger;

        public AppDbController(ILogger<AppDbController> logger, IAppDbService appDbService)
        {
            this.logger = logger;
            this.appDbService = appDbService;
        }

        [HttpPost("{appName}/db")]
        public async Task<IActionResult> Create(string appName, DbMetadataViewModel viewModel)
        {
            if(viewModel == null)
            {
                return BadRequest("DbMetadataViewModel can't be null or empty!");
            }

            await appDbService.CreateDbAsync(appName, viewModel);

            return Created("", $"Database and user created for {appName}");
        }

        [HttpDelete("{appName}/db")]
        public async Task<IActionResult> Delete(string appName)
        {
            return Ok();
        }
    }
}
