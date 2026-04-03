using IoT.Api.Application.DTOs;

namespace IoT.Api.Application.Services
{
    public interface IAlertService { Task<IEnumerable<AlertDto>> GetAlertsAsync(); }
}