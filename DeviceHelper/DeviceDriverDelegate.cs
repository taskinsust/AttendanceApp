using AttendanceApp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.DeviceHelper
{
    public delegate DateTime? GetStandardTime();
    public delegate void MessagePass(string message, MessageType messageType);
}
