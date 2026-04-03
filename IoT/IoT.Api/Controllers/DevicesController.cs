// Controllers/DevicesController.cs
using IoT.Api.Application.DTOs;
using IoT.Api.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        // GET: api/devices
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var devices = await _deviceService.GetAllDevicesAsync();
            return Ok(devices);
        }

        // GET: api/devices/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);
            if (device == null) return NotFound(new { message = $"Không tìm thấy thiết bị ID {id}" });

            return Ok(device);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDeviceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var createdDevice = await _deviceService.CreateDeviceAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = createdDevice.DeviceId }, createdDevice);
            }
            catch (Exception ex)
            {
                // Có thể dính lỗi khóa ngoại nếu MachineId không tồn tại trong database
                return BadRequest(new { message = "Lỗi khi tạo thiết bị. Hãy kiểm tra xem MachineId đã tồn tại chưa.", details = ex.InnerException?.Message ?? ex.Message });
            }
        }

        // PUT: api/devices/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDeviceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updated = await _deviceService.UpdateDeviceAsync(id, request);
                if (!updated) return NotFound(new { message = $"Không tìm thấy thiết bị ID {id}" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi cập nhật.", details = ex.InnerException?.Message ?? ex.Message });
            }
        }

        // DELETE: api/devices/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _deviceService.DeleteDeviceAsync(id);
            if (!deleted) return NotFound(new { message = $"Không tìm thấy thiết bị ID {id}" });

            return NoContent();
        }
    }
}