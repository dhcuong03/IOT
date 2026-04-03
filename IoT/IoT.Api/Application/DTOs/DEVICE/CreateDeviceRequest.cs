namespace IoT.Api.Application.DTOs
{
    public class CreateDeviceRequest
    {
        public string DeviceCode { get; set; }
        public int MachineId { get; set; } // Phải truyền đúng ID của Machine đã tồn tại
        public string Protocol { get; set; }
        public string IpAddress { get; set; }
        public int? Port { get; set; }
        public string Status { get; set; } = "DISCONNECTED"; // Gán mặc định
    }
}