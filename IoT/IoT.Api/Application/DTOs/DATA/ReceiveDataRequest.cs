namespace IoT.Api.Application.DTOs
{
    public class ReceiveDataRequest
    {
        public int DeviceId { get; set; }
        // Dữ liệu JSON gửi lên dưới dạng chuỗi (Ví dụ: "{\"temperature\": 25.5, \"humidity\": 60}")
        public string DataJson { get; set; }
    }
}