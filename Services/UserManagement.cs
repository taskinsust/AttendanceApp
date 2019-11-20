using AttendanceApp.DeviceHelper;
using AttendanceApp.Helper;
using AttendanceApp.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.Services
{
    public class UserManagement
    {
        #region Declaration

        private static readonly ILog serviceLog = LogManager.GetLogger("ServiceLogger");
        private readonly AttendanceDeviceModel _attendanceDevice;
        public readonly ServiceStatus _serviceStatus;
        private MessagePass ThrowMessage;
        private IAttendanceDeviceDriver _attendanceDeviceDriver;

        #endregion

        #region Constructor

        public UserManagement(AttendanceDeviceModel attendanceDevice, IAttendanceDeviceDriver attendanceDeviceDriver, ServiceStatus serviceStatus, MessagePass throwMessage)
        {
            this._attendanceDevice = attendanceDevice;
            this._attendanceDeviceDriver = attendanceDeviceDriver;
            this._serviceStatus = serviceStatus;
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

        //Register user to device
        internal bool RegisterUser()
        {
            try
            {
                //User add
                foreach (var memberViewModel in _attendanceDevice.TeamMembers)
                {
                    if (_serviceStatus.IsRunning == false)
                        return false;

                    var isSuccess = _attendanceDeviceDriver.SaveToDevice(memberViewModel, false);
                    if (isSuccess)
                    {
                        OnThrowingMessage("Member " + memberViewModel.member_id + " operation completed ", MessageType.Success);
                    }
                }

                //Refresh data at device
                _attendanceDeviceDriver.RefreshData();

                return true;
            }
            catch (Exception ex)
            {
                //Log.Error(ex);
                return false;
            }
        }

        internal bool UnRegisterUser()
        {
            try
            {
                //User Delete
                foreach (var item in _attendanceDevice.LoginResponse)
                {
                    if (_serviceStatus.IsRunning == false)
                        return false;

                    var isSuccess = _attendanceDeviceDriver.DeleteFromDevice(item, false);
                    if (isSuccess)
                    {
                        OnThrowingMessage("Member " + item + " operation completed ", MessageType.Success);
                    }
<<<<<<< HEAD
=======
                    else OnThrowingMessage("Member " + item + " operation not valid! Either member does not exit in device or device connection issues ", MessageType.Attention);
>>>>>>> d456686be0805f8de294a31789d9df9ab2cd431e
                }

                //Refresh data at device
                _attendanceDeviceDriver.RefreshData();

                return true;
            }
            catch (Exception ex)
            {
                //Log.Error(ex);
                return false;
            }
        }

        #endregion
    }
}
