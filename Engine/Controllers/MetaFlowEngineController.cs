using Microsoft.AspNetCore.Mvc;

namespace Engine.Controllers
{
    [ApiController]
    [Route("{app}/api/{version}")]
    public class MetaFlowEngineController : Controller
    {
        private readonly ILogger<MetaFlowEngineController> _logger;

        public MetaFlowEngineController(ILogger<MetaFlowEngineController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{**dynamicRoutes}")]
        public IActionResult Get(string app, string version, string dynamicRoute)
        {
            var route = System.Net.WebUtility.UrlDecode(dynamicRoute);

            this._logger.LogInformation("Received request for App: {app}, Version: {version}, Route: {route}", app, version, route);

            return Ok(new
            {
                Tenant = app,
                Version = version,
                RemainingPath = route
            });
        }
    }
}
