using AttendanceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.DeviceHelper
{
    public interface IAttendanceDeviceDriver
    {
        bool SetTime(GetStandardTime getStandardTime);
        void Clean();
        bool SaveToDevice(ActiveUserResponse memberViewModel, bool useInternalConnection = false, int machineNum = 1);
        void ProcessLogList();
        bool IsAttendanceLogExits();
        bool ClearLog();
        bool OpenConnection(bool isDeviceLock = true);
        void CloseConnection();
        void RefreshData();
        bool DeleteFromDevice(LoginResponse memberViewModel,bool useInternalConnection = false, int machineNum = 0);
        
    }
}
