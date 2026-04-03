// Application/DTOs/MachineDashboardDto.cs
namespace IoT.Api.Application.DTOs
{
    public class MachineDashboardDto
    {
        public int TotalMachines { get; set; }
        public int OnlineCount { get; set; }
        public int OfflineCount { get; set; }
        public int WarningCount { get; set; }
        public int MaintenanceCount { get; set; }
    }
}