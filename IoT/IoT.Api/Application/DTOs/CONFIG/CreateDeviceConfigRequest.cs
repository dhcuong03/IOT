namespace IoT.Api.Application.DTOs
{
    public class CreateDeviceConfigRequest
    {
        public int DeviceId { get; set; } // Khóa ngoại trỏ tới Device
        public string ParamName { get; set; }
        public string ParamValue { get; set; }
    }
}