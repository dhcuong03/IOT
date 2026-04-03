namespace IoT.Api.Application.DTOs
{
    public class DeviceDataDto
    {
        public int DataId { get; set; }
        public int DeviceId { get; set; }
        public string DataJson { get; set; }
        public DateTime? RecordedAt { get; set; }
    }
}