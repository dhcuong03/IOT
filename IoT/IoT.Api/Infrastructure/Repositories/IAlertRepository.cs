using IoT.Api.Domain.Entities;

namespace IoT.Api.Infrastructure.Repositories
{
    public interface IAlertRepository
    {
        Task<IEnumerable<IOT_ALERT>> GetAllAlertsAsync();

        Task AddAsync(IOT_ALERT alert);
    }
}