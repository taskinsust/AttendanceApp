using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.Helper
{
    public class BusinessRules
    {
        public const string AppName = "Attendance App";
        public const string AppVersion = "1.0";
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public static bool DevMode = false;
        public static int BackupNumber = 12;
    }

    public enum DeviceCommunicationType
    {
        //SerialPort = 1,
        Ethernet = 2,
        Usb = 3
    }

    public enum MessageType
    {
        Attention,
        Success,
        Error,
        Info,
        Important,
    }

}
