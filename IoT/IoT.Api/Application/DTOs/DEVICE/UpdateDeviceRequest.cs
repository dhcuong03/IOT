namespace IoT.Api.Application.DTOs
{
    public class UpdateDeviceRequest
    {
        public int MachineId { get; set; }
        public string Protocol { get; set; }
        public string IpAddress { get; set; }
        public int? Port { get; set; }
        public string Status { get; set; }
    }
}