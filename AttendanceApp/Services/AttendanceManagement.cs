using AttendanceApp.DeviceHelper;
using AttendanceApp.Helper;
using AttendanceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.Services
{
    public class AttendanceManagement
    {
        #region Declaration

        public MessagePass ThrowMessage;
        private IAttendanceDeviceDriver _attendanceDeviceDriver;
        private AttendanceDeviceModel _attendanceDevice;

        #endregion

        #region Constructor
        public AttendanceManagement(AttendanceDeviceModel attendanceDevice, IAttendanceDeviceDriver attendanceDeviceDriver, MessagePass throwMessage)
        {
            this._attendanceDevice = attendanceDevice;
            this._attendanceDeviceDriver = attendanceDeviceDriver;
            this.ThrowMessage = throwMessage;
        }

        #endregion

        #region Helper

        protected virtual void OnThrowingMessage(string message, MessageType messageType)
        {
            if (ThrowMessage != null)
            {
                ThrowMessage(message, messageType);
            }
        }

        #endregion

        #region Operation

        internal void UpdateAttendance()
        {
            try
            {
                if (_attendanceDeviceDriver.IsAttendanceLogExits() == false)
                {
                    OnThrowingMessage("No new data exits on device [" + _attendanceDevice.IPAddress + "]",
                        MessageType.Info);
                }
                else
                {
                    _attendanceDeviceDriver.ProcessLogList();
                }
                _attendanceDeviceDriver.CloseConnection();
                OnThrowingMessage("Attendance sync successfully completed ", MessageType.Success);
            }
            catch (Exception e)
            {
                OnThrowingMessage("Unable to sync attendance data ", MessageType.Error);
                //Log.Error(e);
            }
        }

        #endregion
    }
}
