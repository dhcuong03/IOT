// Application/Services/MachineService.cs
using IoT.Api.Application.DTOs;
using IoT.Api.Application.DTOs.MACHINE;
using IoT.Api.Domain.Entities;
using IoT.Api.Infrastructure.Repositories;

namespace IoT.Api.Application.Services
{
    public class MachineService : IMachineService
    {
        private readonly IMachineRepository _machineRepository;

        public MachineService(IMachineRepository machineRepository)
        {
            _machineRepository = machineRepository;
        }

        public async Task<IEnumerable<MachineDto>> GetAllMachinesAsync()
        {
            var machines = await _machineRepository.GetAllAsync();

            return machines.Select(m => new MachineDto
            {
                // ÉP KIỂU (int) Ở ĐÂY
                MachineId = (int)m.MACHINE_ID,

                MachineCode = m.MACHINE_CODE,
                MachineName = m.MACHINE_NAME,
                MachineType = m.MACHINE_TYPE,
                Location = m.LOCATION,
                Status = m.STATUS
            });
        }

        // Application/Services/MachineService.cs
        // Thêm hàm này vào trong class MachineService
        public async Task<MachineDto> GetMachineByIdAsync(int id)
        {
            var m = await _machineRepository.GetByIdAsync(id);

            // Nếu không tìm thấy trong DB thì trả về null
            if (m == null) return null;

            // Map dữ liệu sang DTO giống hệt như hàm GetAll
            return new MachineDto
            {
                MachineId = (int)m.MACHINE_ID, // Vẫn nhớ ép kiểu (int) ở đây nhé
                MachineCode = m.MACHINE_CODE,
                MachineName = m.MACHINE_NAME,
                MachineType = m.MACHINE_TYPE,
                Location = m.LOCATION,
                Status = m.STATUS
            };
        }

        public async Task<MachineDto> CreateMachineAsync(CreateMachineRequest request)
        {
            // 1. Map DTO sang Entity
            var newMachine = new MACHINE
            {
                MACHINE_CODE = request.MachineCode,
                MACHINE_NAME = request.MachineName,
                MACHINE_TYPE = request.MachineType,
                LOCATION = request.Location,
                STATUS = request.Status,
                CREATED_AT = DateTime.UtcNow // Ghi nhận thời gian tạo
            };

            // 2. Lưu vào Database
            var createdMachine = await _machineRepository.AddAsync(newMachine);

            // 3. Map Entity vừa tạo trả ngược lại thành DTO
            return new MachineDto
            {
                MachineId = (int)createdMachine.MACHINE_ID,
                MachineCode = createdMachine.MACHINE_CODE,
                MachineName = createdMachine.MACHINE_NAME,
                MachineType = createdMachine.MACHINE_TYPE,
                Location = createdMachine.LOCATION,
                Status = createdMachine.STATUS
            };
        }

        public async Task<bool> UpdateMachineAsync(int id, UpdateMachineRequest request)
        {
            var machine = await _machineRepository.GetByIdAsync(id);
            if (machine == null) return false;

            // Cập nhật các thông tin mới
            machine.MACHINE_NAME = request.MachineName;
            machine.MACHINE_TYPE = request.MachineType;
            machine.LOCATION = request.Location;
            machine.STATUS = request.Status;

            await _machineRepository.UpdateAsync(machine);
            return true;
        }

        public async Task<bool> DeleteMachineAsync(int id)
        {
            var machine = await _machineRepository.GetByIdAsync(id);
            if (machine == null) return false;

            await _machineRepository.DeleteAsync(machine);
            return true;
        }

        public async Task<MachineDashboardDto> GetDashboardAsync()
        {
            // Lấy toàn bộ danh sách máy
            var machines = await _machineRepository.GetAllAsync();

            // Dùng LINQ để đếm số lượng theo từng trạng thái
            return new MachineDashboardDto
            {
                TotalMachines = machines.Count(),
                OnlineCount = machines.Count(m => m.STATUS == "ONLINE"),
                OfflineCount = machines.Count(m => m.STATUS == "OFFLINE"),
                WarningCount = machines.Count(m => m.STATUS == "WARNING"),
                MaintenanceCount = machines.Count(m => m.STATUS == "MAINTENANCE")
            };
        }
    }
}