using IoT.Api.Domain.Entities;

namespace IoT.Api.Infrastructure.Repositories
{
    public interface IConnectionLogRepository
    {
        Task AddAsync(IOT_CONNECTION_LOG log);
    }
}