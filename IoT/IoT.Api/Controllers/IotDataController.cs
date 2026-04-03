// Controllers/IotDataController.cs
using Microsoft.AspNetCore.Mvc;
using IoT.Api.Application.DTOs;
using IoT.Api.Application.Services;

namespace IoT.Api.Controllers
{
    [Route("api/iot")] // Đổi route gốc thành api/iot
    [ApiController]
    public class IotDataController : ControllerBase
    {
        private readonly IIotDataService _dataService;

        public IotDataController(IIotDataService dataService)
        {
            _dataService = dataService;
        }

        // POST: api/iot/data
        [HttpPost("data")]
        public async Task<IActionResult> ReceiveData([FromBody] ReceiveDataRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var savedData = await _dataService.SaveDataAsync(request);
                return Ok(savedData); // Trả về 200 OK kèm theo data đã lưu
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi lưu dữ liệu. Kiểm tra DeviceId hoặc định dạng JSON.", details = ex.InnerException?.Message ?? ex.Message });
            }
        }

        // GET: api/iot/device/{id}/latest
        [HttpGet("device/{id}/latest")]
        public async Task<IActionResult> GetLatest(int id)
        {
            var latestData = await _dataService.GetLatestDataAsync(id);
            if (latestData == null) return NotFound(new { message = $"Chưa có dữ liệu nào cho thiết bị ID {id}" });

            return Ok(latestData);
        }

        // GET: api/iot/device/{id}/history
        [HttpGet("device/{id}/history")]
        public async Task<IActionResult> GetHistory(int id, [FromQuery] int limit = 50)
        {
            // Có thể truyền thêm ?limit=100 trên URL để lấy nhiều dòng hơn
            var historyData = await _dataService.GetHistoryDataAsync(id, limit);
            return Ok(historyData);
        }
    }
}