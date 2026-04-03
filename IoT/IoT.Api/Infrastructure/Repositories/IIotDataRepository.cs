// Infrastructure/Repositories/IIotDataRepository.cs
using IoT.Api.Domain.Entities;

namespace IoT.Api.Infrastructure.Repositories
{
    public interface IIotDataRepository
    {
        Task<IOT_DEVICE_DATum> AddAsync(IOT_DEVICE_DATum data);
        Task<IOT_DEVICE_DATum> GetLatestByDeviceIdAsync(int deviceId);
        Task<IEnumerable<IOT_DEVICE_DATum>> GetHistoryByDeviceIdAsync(int deviceId, int limit = 50);
    }
}