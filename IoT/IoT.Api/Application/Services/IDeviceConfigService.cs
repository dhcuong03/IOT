// Application/Services/IDeviceConfigService.cs
using IoT.Api.Application.DTOs;

namespace IoT.Api.Application.Services
{
    public interface IDeviceConfigService
    {
        Task<IEnumerable<DeviceConfigDto>> GetConfigsByDeviceIdAsync(int deviceId);
        Task<DeviceConfigDto> CreateConfigAsync(CreateDeviceConfigRequest request);
        Task<bool> UpdateConfigAsync(int id, UpdateDeviceConfigRequest request);
        Task<bool> DeleteConfigAsync(int id);
    }
}