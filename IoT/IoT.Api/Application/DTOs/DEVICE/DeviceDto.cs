// Application/DTOs/DeviceDto.cs
namespace IoT.Api.Application.DTOs
{
    public class DeviceDto
    {
        public int DeviceId { get; set; }
        public string DeviceCode { get; set; }
        public int MachineId { get; set; } // Khóa ngoại liên kết với Machine
        public string Protocol { get; set; }
        public string IpAddress { get; set; }
        public int? Port { get; set; }
        public string Status { get; set; }
        public DateTime? LastConnected { get; set; }
    }
}