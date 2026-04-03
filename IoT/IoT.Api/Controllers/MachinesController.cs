// Controllers/MachinesController.cs
using IoT.Api.Application.DTOs;
using IoT.Api.Application.DTOs.MACHINE;
using IoT.Api.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachinesController : ControllerBase
    {
        private readonly IMachineService _machineService;

        public MachinesController(IMachineService machineService)
        {
            _machineService = machineService;
        }

        // GET: api/machines
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var machines = await _machineService.GetAllMachinesAsync();
            return Ok(machines);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var machine = await _machineService.GetMachineByIdAsync(id);

            // Kiểm tra nếu service trả về null -> HTTP 404 Not Found
            if (machine == null)
            {
                return NotFound(new { message = $"Không tìm thấy máy móc nào có ID = {id}" });
            }

            // Nếu có -> HTTP 200 OK kèm dữ liệu
            return Ok(machine);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboardData = await _machineService.GetDashboardAsync();
            return Ok(dashboardData);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMachineRequest request)
        {
            // Kiểm tra tính hợp lệ của dữ liệu đầu vào (nếu có validate)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdMachine = await _machineService.CreateMachineAsync(request);

            // Trả về mã 201 Created, đồng thời trỏ tới hàm GetById để báo cho Client biết 
            // có thể lấy thông tin máy vừa tạo ở đâu
            return CreatedAtAction(nameof(GetById), new { id = createdMachine.MachineId }, createdMachine);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMachineRequest request)
        {
            var result = await _machineService.UpdateMachineAsync(id, request);
            if (!result) return NotFound(new { message = $"Không tìm thấy máy ID {id} để cập nhật" });

            return NoContent(); // Trả về 204 No Content thành công
        }

        // DELETE: api/machines/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _machineService.DeleteMachineAsync(id);
            if (!result) return NotFound(new { message = $"Không tìm thấy máy ID {id} để xóa" });

            return NoContent(); // Trả về 204 No Content thành công
        }
    }
}