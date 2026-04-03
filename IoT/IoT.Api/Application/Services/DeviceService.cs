// Application/Services/DeviceService.cs
using IoT.Api.Application.DTOs;
using IoT.Api.Domain.Entities;
using IoT.Api.Infrastructure.Repositories;

namespace IoT.Api.Application.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<IEnumerable<DeviceDto>> GetAllDevicesAsync()
        {
            var devices = await _deviceRepository.GetAllAsync();

            return devices.Select(d => new DeviceDto
            {
                DeviceId = (int)d.DEVICE_ID,
                DeviceCode = d.DEVICE_CODE,
                MachineId = (int)d.MACHINE_ID,
                Protocol = d.PROTOCOL,
                IpAddress = d.IP_ADDRESS,
                Port = d.PORT != null ? (int)d.PORT : null,
                Status = d.STATUS,
                LastConnected = d.LAST_CONNECTED
            });
        }

        public async Task<DeviceDto> GetDeviceByIdAsync(int id)
        {
            var d = await _deviceRepository.GetByIdAsync(id);
            if (d == null) return null;

            return new DeviceDto
            {
                DeviceId = (int)d.DEVICE_ID,
                DeviceCode = d.DEVICE_CODE,
                MachineId = (int)d.MACHINE_ID,
                Protocol = d.PROTOCOL,
                IpAddress = d.IP_ADDRESS,
                Port = d.PORT != null ? (int)d.PORT : null,
                Status = d.STATUS,
                LastConnected = d.LAST_CONNECTED
            };
        }

        public async Task<DeviceDto> CreateDeviceAsync(CreateDeviceRequest request)
        {
            var newDevice = new IOT_DEVICE
            {
                DEVICE_CODE = request.DeviceCode,
                MACHINE_ID = request.MachineId,
                PROTOCOL = request.Protocol,
                IP_ADDRESS = request.IpAddress,
                PORT = request.Port,
                STATUS = request.Status,
                CREATED_AT = DateTime.UtcNow
            };

            var createdDevice = await _deviceRepository.AddAsync(newDevice);

            return new DeviceDto
            {
                DeviceId = (int)createdDevice.DEVICE_ID,
                DeviceCode = createdDevice.DEVICE_CODE,
                MachineId = (int)createdDevice.MACHINE_ID,
                Protocol = createdDevice.PROTOCOL,
                IpAddress = createdDevice.IP_ADDRESS,
                Port = createdDevice.PORT != null ? (int)createdDevice.PORT : null,
                Status = createdDevice.STATUS,
                LastConnected = createdDevice.LAST_CONNECTED
            };
        }

        public async Task<bool> UpdateDeviceAsync(int id, UpdateDeviceRequest request)
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            if (device == null) return false;

            device.MACHINE_ID = request.MachineId;
            device.PROTOCOL = request.Protocol;
            device.IP_ADDRESS = request.IpAddress;
            device.PORT = request.Port;
            device.STATUS = request.Status;

            await _deviceRepository.UpdateAsync(device);
            return true;
        }

        public async Task<bool> DeleteDeviceAsync(int id)
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            if (device == null) return false;

            await _deviceRepository.DeleteAsync(device);
            return true;
        }
    }
}