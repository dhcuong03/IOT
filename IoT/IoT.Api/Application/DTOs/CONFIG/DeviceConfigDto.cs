namespace IoT.Api.Application.DTOs
{
    public class DeviceConfigDto
    {
        public int ConfigId { get; set; }
        public int DeviceId { get; set; }
        public string ParamName { get; set; }
        public string ParamValue { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}