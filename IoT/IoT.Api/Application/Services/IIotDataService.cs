// Application/Services/IIotDataService.cs
using IoT.Api.Application.DTOs;

namespace IoT.Api.Application.Services
{
    public interface IIotDataService
    {
        Task<DeviceDataDto> SaveDataAsync(ReceiveDataRequest request);
        Task<DeviceDataDto> GetLatestDataAsync(int deviceId);
        Task<IEnumerable<DeviceDataDto>> GetHistoryDataAsync(int deviceId, int limit = 50);
    }
}