// Infrastructure/Repositories/DeviceRepository.cs
using Microsoft.EntityFrameworkCore;
using IoT.Api.Domain.Entities;
using IoT.Api.Infrastructure.Data;

namespace IoT.Api.Infrastructure.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly AppDbContext _context;

        public DeviceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IOT_DEVICE>> GetAllAsync()
        {
            // Vẫn dùng .Set<T>() để tránh lỗi map sai tên bảng
            return await _context.Set<IOT_DEVICE>().ToListAsync();
        }

        public async Task<IOT_DEVICE> GetByIdAsync(int id)
        {
            return await _context.Set<IOT_DEVICE>()
                                 .FirstOrDefaultAsync(d => d.DEVICE_ID == id);
        }

        public async Task<IOT_DEVICE> AddAsync(IOT_DEVICE device)
        {
            await _context.Set<IOT_DEVICE>().AddAsync(device);
            await _context.SaveChangesAsync();
            return device;
        }

        public async Task UpdateAsync(IOT_DEVICE device)
        {
            _context.Set<IOT_DEVICE>().Update(device);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(IOT_DEVICE device)
        {
            _context.Set<IOT_DEVICE>().Remove(device);
            await _context.SaveChangesAsync();
        }
    }
}