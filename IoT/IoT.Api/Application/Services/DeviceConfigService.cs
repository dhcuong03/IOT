// Application/Services/DeviceConfigService.cs
using IoT.Api.Application.DTOs;
using IoT.Api.Domain.Entities;
using IoT.Api.Infrastructure.Repositories;

namespace IoT.Api.Application.Services
{
    public class DeviceConfigService : IDeviceConfigService
    {
        private readonly IDeviceConfigRepository _configRepository;

        public DeviceConfigService(IDeviceConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public async Task<IEnumerable<DeviceConfigDto>> GetConfigsByDeviceIdAsync(int deviceId)
        {
            var configs = await _configRepository.GetByDeviceIdAsync(deviceId);
            return configs.Select(c => new DeviceConfigDto
            {
                ConfigId = (int)c.CONFIG_ID,
                DeviceId = (int)c.DEVICE_ID,
                ParamName = c.PARAM_NAME,
                ParamValue = c.PARAM_VALUE,
                UpdatedAt = c.UPDATED_AT
            });
        }

        public async Task<DeviceConfigDto> CreateConfigAsync(CreateDeviceConfigRequest request)
        {
            var newConfig = new IOT_DEVICE_CONFIG
            {
                DEVICE_ID = request.DeviceId,
                PARAM_NAME = request.ParamName,
                PARAM_VALUE = request.ParamValue,
                UPDATED_AT = DateTime.UtcNow // Ghi nhận thời gian tạo
            };

            var createdConfig = await _configRepository.AddAsync(newConfig);

            return new DeviceConfigDto
            {
                ConfigId = (int)createdConfig.CONFIG_ID,
                DeviceId = (int)createdConfig.DEVICE_ID,
                ParamName = createdConfig.PARAM_NAME,
                ParamValue = createdConfig.PARAM_VALUE,
                UpdatedAt = createdConfig.UPDATED_AT
            };
        }

        public async Task<bool> UpdateConfigAsync(int id, UpdateDeviceConfigRequest request)
        {
            var config = await _configRepository.GetByIdAsync(id);
            if (config == null) return false;

            config.PARAM_NAME = request.ParamName;
            config.PARAM_VALUE = request.ParamValue;
            config.UPDATED_AT = DateTime.UtcNow; // Cập nhật lại thời gian khi có thay đổi

            await _configRepository.UpdateAsync(config);
            return true;
        }

        public async Task<bool> DeleteConfigAsync(int id)
        {
            var config = await _configRepository.GetByIdAsync(id);
            if (config == null) return false;

            await _configRepository.DeleteAsync(config);
            return true;
        }
    }
}