using Microsoft.AspNetCore.Mvc;

namespace Engine.Controllers
{
    [ApiController]
    [Route("{app}/api/{version}")]
    public class EngineController : Controller
    {
        private readonly ILogger<EngineController> logger;

        public EngineController(ILogger<EngineController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("{**dynamicRoutes}")]
        public IActionResult Get(string app, string version, string dynamicRoute)
        {
            var route = System.Net.WebUtility.UrlDecode(dynamicRoute);

            this.logger.LogInformation("Received request for App: {app}, Version: {version}, Route: {route}", app, version, route);

            return Ok(new
            {
                Tenant = app,
                Version = version,
                RemainingPath = route
            });
        }
    }
}
