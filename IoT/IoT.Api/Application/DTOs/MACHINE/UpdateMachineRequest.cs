// Application/DTOs/UpdateMachineRequest.cs
namespace IoT.Api.Application.DTOs
{
    public class UpdateMachineRequest
    {
        public string MachineName { get; set; }
        public string MachineType { get; set; }
        public string Location { get; set; }
        public string Status { get; set; } // Nhớ chỉ nhập: ONLINE, OFFLINE, MAINTENANCE, WARNING
    }
}