using IoT.Api.Application.DTOs;
using IoT.Api.Infrastructure.Repositories;

namespace IoT.Api.Application.Services
{
    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _alertRepo;
        public AlertService(IAlertRepository alertRepo) => _alertRepo = alertRepo;

        public async Task<IEnumerable<AlertDto>> GetAlertsAsync()
        {
            var alerts = await _alertRepo.GetAllAlertsAsync();
            return alerts.Select(a => new AlertDto
            {
                AlertId = (int)a.ALERT_ID,
                DeviceId = a.DEVICE_ID != null ? (int)a.DEVICE_ID : null,
                MetricCode = a.METRIC_CODE,
                AlertType = a.ALERT_TYPE,
                Message = a.MESSAGE,
                CreatedAt = a.CREATED_AT
            });
        }
    }
}