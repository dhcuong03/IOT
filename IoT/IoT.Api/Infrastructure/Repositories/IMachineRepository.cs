// Infrastructure/Repositories/IMachineRepository.cs
using IoT.Api.Domain.Entities;

namespace IoT.Api.Infrastructure.Repositories
{
    public interface IMachineRepository
    {
        Task<IEnumerable<MACHINE>> GetAllAsync();

        Task<MACHINE> GetByIdAsync(int id);

        Task<MACHINE> AddAsync(MACHINE machine);
        Task UpdateAsync(MACHINE machine);
        Task DeleteAsync(MACHINE machine);
    }
}