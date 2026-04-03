// Infrastructure/Repositories/IDeviceConfigRepository.cs
using IoT.Api.Domain.Entities;

namespace IoT.Api.Infrastructure.Repositories
{
    public interface IDeviceConfigRepository
    {
        Task<IEnumerable<IOT_DEVICE_CONFIG>> GetByDeviceIdAsync(int deviceId);
        Task<IOT_DEVICE_CONFIG> GetByIdAsync(int id);
        Task<IOT_DEVICE_CONFIG> AddAsync(IOT_DEVICE_CONFIG config);
        Task UpdateAsync(IOT_DEVICE_CONFIG config);
        Task DeleteAsync(IOT_DEVICE_CONFIG config);
    }
}