using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.Models
{
    public class AttendanceDeviceModel
    {
        public int Id { get; set; }
        public virtual string DeviceTypeCode { get; set; }
        public string Name { get; set; }
        public string DeviceModelNo { get; set; }

        public string IPAddress { get; set; }
        public int Port { get; set; }
        public string CommunicationKey { get; set; }
        public int MachineNo { get; set; }
        public List<ActiveUserResponse> TeamMembers { get; set; }
        public List<LoginResponse> LoginResponse { get; set; }
        // public int VersionNumber { get; set; }
        public bool IsReset { get; set; }
        public string LastUpdateTime { get; set; }

        public int BranchId { get; set; }
    }
}
