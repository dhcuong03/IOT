// Controllers/HeartbeatController.cs
using Microsoft.AspNetCore.Mvc;
using IoT.Api.Application.DTOs;
using IoT.Api.Application.Services;

namespace IoT.Api.Controllers
{
    [Route("api/iot/heartbeat")]
    [ApiController]
    public class HeartbeatController : ControllerBase
    {
        private readonly IHeartbeatService _heartbeatService;

        public HeartbeatController(IHeartbeatService heartbeatService)
        {
            _heartbeatService = heartbeatService;
        }

        [HttpPost]
        public async Task<IActionResult> PostHeartbeat([FromBody] HeartbeatRequest request)
        {
            var success = await _heartbeatService.ProcessHeartbeatAsync(request);
            if (!success) return NotFound(new { message = $"Không tìm thấy thiết bị ID {request.DeviceId}" });

            return Ok(new { message = "Heartbeat processed successfully." });
        }
    }
}