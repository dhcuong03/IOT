// Application/Services/IDeviceService.cs
using IoT.Api.Application.DTOs;

namespace IoT.Api.Application.Services
{
    public interface IDeviceService
    {
        Task<IEnumerable<DeviceDto>> GetAllDevicesAsync();
        Task<DeviceDto> GetDeviceByIdAsync(int id);

        Task<DeviceDto> CreateDeviceAsync(CreateDeviceRequest request);
        Task<bool> UpdateDeviceAsync(int id, UpdateDeviceRequest request);
        Task<bool> DeleteDeviceAsync(int id);
    }
}