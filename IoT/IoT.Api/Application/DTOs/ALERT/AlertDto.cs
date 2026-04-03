// Application/DTOs/AlertDto.cs
namespace IoT.Api.Application.DTOs
{
    public class AlertDto
    {
        public int AlertId { get; set; }
        public int? DeviceId { get; set; }
        public string MetricCode { get; set; }
        public string AlertType { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}