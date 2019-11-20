using AttendanceApp.DeviceHelper;
using AttendanceApp.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.Services
{
    public class DeviceManagement
    {
        #region Declaration

        private static readonly ILog serviceLog = LogManager.GetLogger("ServiceLogger");
        private readonly AttendanceDeviceModel _attendanceDevice;
        private readonly IAttendanceDeviceDriver _attendanceDeviceDriver;

        #endregion

        #region Constructor

        public DeviceManagement(AttendanceDeviceModel attendanceDevice, IAttendanceDeviceDriver attendanceDeviceDriver)
        {
            this._attendanceDevice = attendanceDevice;
            _attendanceDeviceDriver = attendanceDeviceDriver;
        }

        #endregion

        #region Helper
        public DateTime GetCurrentTime()
        {
            return new DateTime();
        }

        #endregion

        #region Operation

        //Prepare device before operation
        // clean Device hardware
        internal bool ReadyDevice()
        {
            try
            {
                _attendanceDeviceDriver.Clean();
                _attendanceDeviceDriver.RefreshData();

                return true;
            }
            catch (Exception e)
            {
                //Log.Error(e);
                return false;
            }
        }

        #endregion
    }
}
