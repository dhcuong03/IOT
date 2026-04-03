// Infrastructure/Repositories/IDeviceRepository.cs
using IoT.Api.Domain.Entities;

namespace IoT.Api.Infrastructure.Repositories
{
    public interface IDeviceRepository
    {
        Task<IEnumerable<IOT_DEVICE>> GetAllAsync();
        Task<IOT_DEVICE> GetByIdAsync(int id);

        Task<IOT_DEVICE> AddAsync(IOT_DEVICE device);
        Task UpdateAsync(IOT_DEVICE device);
        Task DeleteAsync(IOT_DEVICE device);
    }
}