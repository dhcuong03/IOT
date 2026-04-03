using IoT.Api.Application.DTOs;

namespace IoT.Api.Application.Services
{
    public interface IHeartbeatService
    {
        Task<bool> ProcessHeartbeatAsync(HeartbeatRequest request);
    }
}