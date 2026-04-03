using Microsoft.AspNetCore.Mvc;
using IoT.Api.Application.Services;

namespace IoT.Api.Controllers
{
    [Route("api/iot/alerts")]
    [ApiController]
    public class AlertsController : ControllerBase
    {
        private readonly IAlertService _alertService;
        public AlertsController(IAlertService alertService) => _alertService = alertService;

        [HttpGet]
        public async Task<IActionResult> GetAlerts()
        {
            var alerts = await _alertService.GetAlertsAsync();
            return Ok(alerts);
        }
    }
}