using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.Models
{
    public class DeviceTypes
    {
        public string DeviceIp { get; set; }
        public int Port { get; set; }
        public string DeviceType { get; set; }
        public int BranchId { get; set; }
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public bool IsConDevice { get; set; } = false;
    }
}
