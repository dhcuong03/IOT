using IoT.Api.Application.DTOs;
using IoT.Api.Domain.Entities;
using IoT.Api.Infrastructure.Repositories;

namespace IoT.Api.Application.Services
{
    public class HeartbeatService : IHeartbeatService
    {
        private readonly IDeviceRepository _deviceRepo;
        private readonly IConnectionLogRepository _logRepo;

        public HeartbeatService(IDeviceRepository deviceRepo, IConnectionLogRepository logRepo)
        {
            _deviceRepo = deviceRepo;
            _logRepo = logRepo;
        }

        public async Task<bool> ProcessHeartbeatAsync(HeartbeatRequest request)
        {
            // 1. Tìm thiết bị
            var device = await _deviceRepo.GetByIdAsync(request.DeviceId);
            if (device == null) return false;

            // 2. Cập nhật trạng thái và thời gian online mới nhất
            device.STATUS = "CONNECTED";
            device.LAST_CONNECTED = DateTime.UtcNow;
            await _deviceRepo.UpdateAsync(device);

            // 3. Ghi log kết nối
            var log = new IOT_CONNECTION_LOG
            {
                DEVICE_ID = request.DeviceId,
                STATUS = "CONNECTED",
                MESSAGE = "Heartbeat received",
                CREATED_AT = DateTime.UtcNow
            };
            await _logRepo.AddAsync(log);

            return true;
        }
    }
}