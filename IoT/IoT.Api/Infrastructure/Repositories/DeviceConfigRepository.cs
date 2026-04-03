// Infrastructure/Repositories/DeviceConfigRepository.cs
using Microsoft.EntityFrameworkCore;
using IoT.Api.Domain.Entities;
using IoT.Api.Infrastructure.Data;

namespace IoT.Api.Infrastructure.Repositories
{
    public class DeviceConfigRepository : IDeviceConfigRepository
    {
        private readonly AppDbContext _context;

        public DeviceConfigRepository(AppDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách cấu hình của một thiết bị cụ thể
        public async Task<IEnumerable<IOT_DEVICE_CONFIG>> GetByDeviceIdAsync(int deviceId)
        {
            return await _context.Set<IOT_DEVICE_CONFIG>()
                                 .Where(c => c.DEVICE_ID == deviceId)
                                 .ToListAsync();
        }

        public async Task<IOT_DEVICE_CONFIG> GetByIdAsync(int id)
        {
            return await _context.Set<IOT_DEVICE_CONFIG>()
                                 .FirstOrDefaultAsync(c => c.CONFIG_ID == id);
        }

        public async Task<IOT_DEVICE_CONFIG> AddAsync(IOT_DEVICE_CONFIG config)
        {
            await _context.Set<IOT_DEVICE_CONFIG>().AddAsync(config);
            await _context.SaveChangesAsync();
            return config;
        }

        public async Task UpdateAsync(IOT_DEVICE_CONFIG config)
        {
            _context.Set<IOT_DEVICE_CONFIG>().Update(config);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(IOT_DEVICE_CONFIG config)
        {
            _context.Set<IOT_DEVICE_CONFIG>().Remove(config);
            await _context.SaveChangesAsync();
        }
    }
}