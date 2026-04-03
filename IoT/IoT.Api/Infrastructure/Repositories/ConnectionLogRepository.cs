using IoT.Api.Domain.Entities;
using IoT.Api.Infrastructure.Data;

namespace IoT.Api.Infrastructure.Repositories
{
    public class ConnectionLogRepository : IConnectionLogRepository
    {
        private readonly AppDbContext _context;
        public ConnectionLogRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(IOT_CONNECTION_LOG log)
        {
            await _context.Set<IOT_CONNECTION_LOG>().AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}