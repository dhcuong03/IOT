using System.Text.Json; // Thêm thư viện này để đọc JSON
using IoT.Api.Application.DTOs;
using IoT.Api.Domain.Entities;
using IoT.Api.Infrastructure.Repositories;

namespace IoT.Api.Application.Services
{
    public class IotDataService : IIotDataService
    {
        private readonly IIotDataRepository _dataRepository;
        private readonly IAlertRepository _alertRepository; // Thêm Alert Repo

        // Tiêm cả 2 Repository vào Constructor
        public IotDataService(IIotDataRepository dataRepository, IAlertRepository alertRepository)
        {
            _dataRepository = dataRepository;
            _alertRepository = alertRepository;
        }

        public async Task<DeviceDataDto> SaveDataAsync(ReceiveDataRequest request)
        {
            // 1. Vẫn lưu data bình thường vào bảng IOT_DEVICE_DATA
            var newData = new IOT_DEVICE_DATum
            {
                DEVICE_ID = request.DeviceId,
                DATA_JSON = request.DataJson,
                RECORDED_AT = DateTime.UtcNow
            };

            var saved = await _dataRepository.AddAsync(newData);

            // 2. PHÂN TÍCH DATA ĐỂ TẠO CẢNH BÁO TỰ ĐỘNG
            try
            {
                // Parse chuỗi JSON thiết bị gửi lên
                using JsonDocument doc = JsonDocument.Parse(request.DataJson);

                // Kiểm tra xem trong JSON có thuộc tính "temperature" không
                if (doc.RootElement.TryGetProperty("temperature", out JsonElement tempElement))
                {
                    double tempValue = tempElement.GetDouble();

                    // Nếu nhiệt độ lớn hơn 80 thì lưu một cảnh báo vào database
                    if (tempValue > 80.0)
                    {
                        var alert = new IOT_ALERT
                        {
                            DEVICE_ID = request.DeviceId,
                            METRIC_CODE = "TEMPERATURE",
                            ALERT_TYPE = "OVER_THRESHOLD",
                            MESSAGE = $"CẢNH BÁO KHẨN: Nhiệt độ đạt mức {tempValue}°C (Vượt ngưỡng 80°C)",
                            CREATED_AT = DateTime.UtcNow
                        };
                        await _alertRepository.AddAsync(alert);
                    }
                }
            }
            catch
            {
                // Bỏ qua lỗi nếu JSON format sai (để không làm gián đoạn việc trả kết quả lưu data)
            }

            // 3. Trả về kết quả
            return new DeviceDataDto
            {
                DataId = (int)saved.DATA_ID,
                DeviceId = (int)saved.DEVICE_ID,
                DataJson = saved.DATA_JSON,
                RecordedAt = saved.RECORDED_AT
            };
        }

        public async Task<DeviceDataDto> GetLatestDataAsync(int deviceId)
        {
            var data = await _dataRepository.GetLatestByDeviceIdAsync(deviceId);
            if (data == null) return null;

            return new DeviceDataDto
            {
                DataId = (int)data.DATA_ID,
                DeviceId = (int)data.DEVICE_ID,
                DataJson = data.DATA_JSON,
                RecordedAt = data.RECORDED_AT
            };
        }

        public async Task<IEnumerable<DeviceDataDto>> GetHistoryDataAsync(int deviceId, int limit = 50)
        {
            var dataList = await _dataRepository.GetHistoryByDeviceIdAsync(deviceId, limit);

            return dataList.Select(d => new DeviceDataDto
            {
                DataId = (int)d.DATA_ID,
                DeviceId = (int)d.DEVICE_ID,
                DataJson = d.DATA_JSON,
                RecordedAt = d.RECORDED_AT
            });
        }
    }
}   