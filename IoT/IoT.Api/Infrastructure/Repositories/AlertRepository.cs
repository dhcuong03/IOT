using Microsoft.EntityFrameworkCore;
using IoT.Api.Domain.Entities;
using IoT.Api.Infrastructure.Data;

namespace IoT.Api.Infrastructure.Repositories
{
    public class AlertRepository : IAlertRepository
    {
        private readonly AppDbContext _context;
        public AlertRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<IOT_ALERT>> GetAllAlertsAsync()
        {
            // Lấy danh sách cảnh báo, sắp xếp mới nhất lên đầu
            return await _context.Set<IOT_ALERT>()
                                 .OrderByDescending(a => a.CREATED_AT)
                                 .ToListAsync();
        }

        public async Task AddAsync(IOT_ALERT alert)
        {
            await _context.Set<IOT_ALERT>().AddAsync(alert);
            await _context.SaveChangesAsync();
        }
    }
}