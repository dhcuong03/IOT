// Infrastructure/Repositories/IotDataRepository.cs
using Microsoft.EntityFrameworkCore;
using IoT.Api.Domain.Entities;
using IoT.Api.Infrastructure.Data;

namespace IoT.Api.Infrastructure.Repositories
{
    public class IotDataRepository : IIotDataRepository
    {
        private readonly AppDbContext _context;

        public IotDataRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IOT_DEVICE_DATum> AddAsync(IOT_DEVICE_DATum data)
        {
            await _context.Set<IOT_DEVICE_DATum>().AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<IOT_DEVICE_DATum> GetLatestByDeviceIdAsync(int deviceId)
        {
            // Sắp xếp theo thời gian mới nhất giảm dần và lấy dòng đầu tiên
            return await _context.Set<IOT_DEVICE_DATum>()
                                 .Where(d => d.DEVICE_ID == deviceId)
                                 .OrderByDescending(d => d.RECORDED_AT)
                                 .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<IOT_DEVICE_DATum>> GetHistoryByDeviceIdAsync(int deviceId, int limit = 50)
        {
            // Lấy danh sách lịch sử, giới hạn số lượng (mặc định 50 dòng) để vẽ biểu đồ
            return await _context.Set<IOT_DEVICE_DATum>()
                                 .Where(d => d.DEVICE_ID == deviceId)
                                 .OrderByDescending(d => d.RECORDED_AT)
                                 .Take(limit)
                                 .ToListAsync();
        }
    }
}