// Application/DTOs/CreateMachineRequest.cs
namespace IoT.Api.Application.DTOs.MACHINE
{
    public class CreateMachineRequest
    {
        public string MachineCode { get; set; }
        public string MachineName { get; set; }
        public string MachineType { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
    }
}