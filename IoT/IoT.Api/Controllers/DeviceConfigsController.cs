// Controllers/DeviceConfigsController.cs
using Microsoft.AspNetCore.Mvc;
using IoT.Api.Application.DTOs;
using IoT.Api.Application.Services;

namespace IoT.Api.Controllers
{
    [Route("api/device-config")] // Đặt route giống hệt yêu cầu của bạn
    [ApiController]
    public class DeviceConfigsController : ControllerBase
    {
        private readonly IDeviceConfigService _configService;

        public DeviceConfigsController(IDeviceConfigService configService)
        {
            _configService = configService;
        }

        // GET: api/device-config/{deviceId}
        [HttpGet("{deviceId}")]
        public async Task<IActionResult> GetByDeviceId(int deviceId)
        {
            var configs = await _configService.GetConfigsByDeviceIdAsync(deviceId);
            return Ok(configs);
        }

        // POST: api/device-config
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDeviceConfigRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var created = await _configService.CreateConfigAsync(request);
                // Không cần CreatedAtAction vì get detail thường lấy theo danh sách của device
                return Ok(created); // Trả về 200 OK kèm data vừa tạo
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi tạo cấu hình. Kiểm tra DeviceId.", details = ex.InnerException?.Message ?? ex.Message });
            }
        }

        // PUT: api/device-config/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDeviceConfigRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _configService.UpdateConfigAsync(id, request);
            if (!updated) return NotFound(new { message = $"Không tìm thấy cấu hình ID {id}" });

            return NoContent();
        }

        // DELETE: api/device-config/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _configService.DeleteConfigAsync(id);
            if (!deleted) return NotFound(new { message = $"Không tìm thấy cấu hình ID {id}" });

            return NoContent();
        }
    }
}