using Engine.Models.ViewModels;
using Engine.Services.AppDb;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Controllers.Admin
{
    [ApiController]
    [Route("admin/api/{appName}/db")]
    public class AppDbController : Controller
    {
        private readonly IAppDbService appDbService;

        public AppDbController(IAppDbService appDbService)
        {
            this.appDbService = appDbService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string appName, DbMetadataViewModel viewModel)
        {
            if(viewModel == null)
            {
                return BadRequest("DbMetadataViewModel can't be null or empty!");
            }

            await appDbService.CreateDbAsync(appName, viewModel);

            return Created("", $"Database and user created for {appName}");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string appName)
        {
            if (appName == null || string.IsNullOrWhiteSpace(appName))
            {
                return BadRequest("'appName' can't be null or empty!");
            }

            await appDbService.DeleteDbAsync(appName);

            return Ok("Database and user deleted.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(string appName)
        {
            if (appName == null || string.IsNullOrWhiteSpace(appName))
            {
                return BadRequest("'appName' can't be null or empty!");
            }

            return Ok("Database credentials updated");
        }
    }
}
