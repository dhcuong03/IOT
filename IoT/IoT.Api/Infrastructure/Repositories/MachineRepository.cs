// Infrastructure/Repositories/MachineRepository.cs
using Microsoft.EntityFrameworkCore;
using IoT.Api.Domain.Entities;
using IoT.Api.Infrastructure.Data;

namespace IoT.Api.Infrastructure.Repositories
{
    public class MachineRepository : IMachineRepository
    {
        private readonly AppDbContext _context;

        public MachineRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MACHINE>> GetAllAsync()
        {
            return await _context.Set<MACHINE>().ToListAsync();
        }

        public async Task<MACHINE> GetByIdAsync(int id)
        {
            // Dùng FirstOrDefaultAsync thay vì FindAsync để tránh lỗi mismatch giữa int và decimal
            return await _context.Set<MACHINE>()
                                 .FirstOrDefaultAsync(m => m.MACHINE_ID == id);
        }

        public async Task<MACHINE> AddAsync(MACHINE machine)
        {
            await _context.Set<MACHINE>().AddAsync(machine);
            await _context.SaveChangesAsync(); // Bước này rất quan trọng để thực thi lệnh INSERT và COMMIT
            return machine;
        }

        public async Task UpdateAsync(MACHINE machine)
        {
            _context.Set<MACHINE>().Update(machine);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(MACHINE machine)
        {
            _context.Set<MACHINE>().Remove(machine);
            await _context.SaveChangesAsync();
        }
    }
}