using AttendanceApp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.Services
{
    public class ServiceDelegate
    {
        public delegate void DeviceSynchronizedEventHandler(string message, MessageType messageType);
    }
}
