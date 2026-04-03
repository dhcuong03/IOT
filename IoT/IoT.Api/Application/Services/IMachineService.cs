// Application/Services/IMachineService.cs
using IoT.Api.Application.DTOs;
using IoT.Api.Application.DTOs.MACHINE;
using IoT.Api.Domain.Entities;

namespace IoT.Api.Application.Services
{
    public interface IMachineService
    {
        Task<IEnumerable<MachineDto>> GetAllMachinesAsync();

        Task<MachineDto> GetMachineByIdAsync(int id);

        Task<MachineDto> CreateMachineAsync(CreateMachineRequest request);

        Task<MachineDashboardDto> GetDashboardAsync();

        Task<bool> UpdateMachineAsync(int id, UpdateMachineRequest request);
        Task<bool> DeleteMachineAsync(int id);

    }

}