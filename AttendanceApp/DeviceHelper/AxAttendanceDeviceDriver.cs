using AttendanceApp.Helper;
using AttendanceApp.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zkemkeeper;

namespace AttendanceApp.DeviceHelper
{
    public abstract class AxAttendanceDeviceDriver : IAttendanceDeviceDriver
    {
        protected static readonly ILog serviceLog = LogManager.GetLogger("ServiceLogger");
        protected static CZKEM axDevice;
        protected AttendanceDeviceModel _attendanceDevice;
        protected MessagePass _throwMessage;

        #region Constructor

        public AxAttendanceDeviceDriver(AttendanceDeviceModel attendanceDevice, MessagePass throwMessage)
        {
            this._attendanceDevice = attendanceDevice;
            this._throwMessage = throwMessage;
            axDevice = new CZKEM();
            
        }

        #endregion

        #region Properties

        public CZKEM AxDevice
        {
            get { return axDevice; }
            set { axDevice = value; }
        }

        #endregion

        #region General

        public bool OpenConnection(bool isDeviceLock = true)
        {
            bool isSuccess = false;
            try
            {
                //Connecting with device
                string connectWithMsg = _attendanceDevice.Name;
                AxDevice.SetCommPassword(Convert.ToInt32(_attendanceDevice.CommunicationKey));
                isSuccess = AxDevice.Connect_Net(_attendanceDevice.IPAddress, Convert.ToInt32(_attendanceDevice.Port));
                connectWithMsg += " [" + _attendanceDevice.IPAddress + "]";

                //Locking Device
                if (isSuccess)
                {
                    OnThrowingMessage("Connected with: " + connectWithMsg, MessageType.Attention);
                    if (isDeviceLock)
                    {
                        isSuccess = LockDevice(isDeviceLock);
                        if (isSuccess)
                            OnThrowingMessage("Device Locked ", MessageType.Info);
                    }
                }
                else
                {
                    OnThrowingMessage("Failed to connect with: " + connectWithMsg, MessageType.Error);
                }

            }
            catch (Exception ex)
            {
                isSuccess = false;
                serviceLog.Error(ex);
            }
            return isSuccess;
        }

        public void CloseConnection()
        {
            try
            {
                //unlock device
                bool isSuccess = LockDevice(false);
                if (isSuccess)
                    OnThrowingMessage("Device unlocked ", MessageType.Info);
                else
                    OnThrowingMessage("Unable to unlock device", MessageType.Error);

                //Disconnect Device
                AxDevice.Disconnect();
                string connectWithMsg = _attendanceDevice.Name;
                connectWithMsg += " [" + _attendanceDevice.IPAddress + "]";

                OnThrowingMessage("Connection closed for: " + connectWithMsg, MessageType.Attention);
            }
            catch (Exception ex)
            {
                serviceLog.Error(ex);
            }
        }

        public void RefreshData()
        {
            try
            {
                AxDevice.RefreshData(_attendanceDevice.MachineNo);
                OnThrowingMessage("Refreshed device ", MessageType.Info);
            }
            catch (Exception ex)
            {
                serviceLog.Error(ex);
            }
        }

        #endregion

        #region Device Implementation

        public bool SetTime(GetStandardTime getStandardTime)
        {
            bool isSuccess = false;
            try
            {
                OnThrowingMessage("Updating device time", MessageType.Info);
                var serverTime = getStandardTime();

                if (serverTime == null)
                {
                    throw new Exception("Unable to get server time");
                }
                else
                {
                    var deviceTime = GetTime();
                    var standardTime = serverTime.Value;
                    var stime = new DateTime(standardTime.Year, standardTime.Month, standardTime.Day, standardTime.Hour, standardTime.Minute, 0);
                    if (deviceTime != stime)
                    {
                        //Set Device Time
                        isSuccess = AxDevice.SetDeviceTime2(_attendanceDevice.MachineNo, standardTime.Year, standardTime.Month,
                                            standardTime.Day, standardTime.Hour, standardTime.Minute, standardTime.Second);
                        RefreshData();
                        if (isSuccess == false)
                            throw new Exception("Unable to set time on device");
                    }
                }
            }
            catch (Exception ex)
            {
                //Log.Error(ex);
                OnThrowingMessage(ex.Message, MessageType.Error);
            }
            return isSuccess;
        }

        public void Clean()
        {
            try
            {
                OnThrowingMessage("Clearing device data", MessageType.Info);

                //Clear Attendance Data
                bool isSuccess = AxDevice.ClearData(_attendanceDevice.MachineNo, 1);

                //Clear User Data
                if (isSuccess)
                    AxDevice.ClearData(_attendanceDevice.MachineNo, 5);
            }
            catch (Exception ex)
            {
                //Log.Error(ex);
                OnThrowingMessage("Unable to clear device data", MessageType.Error);
            }
        }

        #endregion

        #region User Implementation

        public bool SaveToDevice(ActiveUserResponse member, bool useInternalConnection = false, int machineNum = 0)
        {
            bool returnResult = false;
            //Open Connection if needed
            if (useInternalConnection)
            {
                bool isSuccess = OpenConnection();
                if (!isSuccess) return false;
            }

            //Business logic
            try
            {
                if (member == null) return false;
                string sPassword = "";
                int iPrivilege = 0;

                returnResult = Register(member, sPassword, iPrivilege);
            }
            catch (Exception ex)
            {
                serviceLog.Error(ex);
                return false;
            }
            finally
            {
                if (useInternalConnection)
                {
                    RefreshData();
                    CloseConnection();
                }
            }
            return returnResult;
        }

        public bool DeleteFromDevice(LoginResponse loginResponse, bool useInternalConnection = false, int machineNum = 0)
        {
            bool returnResult = false;
            //Open Connection if needed
            if (useInternalConnection)
            {
                bool isSuccess = OpenConnection();
                if (!isSuccess) return false;
            }
            //Business logic
            try
            {
                if (loginResponse == null) return false;
                string sPassword = "";
                int iPrivilege = 0;

                returnResult = UnRegister(loginResponse, sPassword, iPrivilege);
            }
            catch (Exception ex)
            {
                serviceLog.Error(ex);
                return false;
            }
            finally
            {
                if (useInternalConnection)
                {
                    RefreshData();
                    CloseConnection();
                }
            }
            return returnResult;
        }
        #endregion

        #region Attendance Implementation

        public bool IsAttendanceLogExits()
        {
            try
            {
                bool isSuc = AxDevice.ReadGeneralLogData(_attendanceDevice.MachineNo);
                return isSuc;
            }
            catch (Exception ex)
            {
                serviceLog.Error(ex);
                return false;
            }
        }

        public bool ClearLog()
        {
            try
            {
                bool isSuc = AxDevice.ClearGLog(_attendanceDevice.MachineNo);
                return isSuc;
            }
            catch (Exception ex)
            {
                serviceLog.Error(ex);
                return false;
            }
        }

        #endregion

        #region Protected methods

        protected bool LockDevice(bool isLock)
        {
            try
            {
                bool setEnable = (isLock == false);
                bool isSuccess = AxDevice.EnableDevice(_attendanceDevice.MachineNo, setEnable);
                return isSuccess;
            }
            catch (Exception ex)
            {
                serviceLog.Error(ex);
                return false;
            }
        }

        protected DateTime GetTime()
        {
            int idwYear = 0;
            int idwMonth = 0;
            int idwDay = 0;
            int idwHour = 0;
            int idwMinute = 0;
            int idwSecond = 0;
            try
            {
                AxDevice.GetDeviceTime(_attendanceDevice.MachineNo, ref idwYear,
                    ref idwMonth, ref idwDay, ref idwHour, ref idwMinute, ref idwSecond);
                if (idwYear == 0) throw new Exception("Unable to get device time");
                var deviceTime = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, 0);
                return deviceTime;
            }
            catch (Exception ex)
            {
                serviceLog.Error(ex);
                throw;
            }
        }

        protected virtual void OnThrowingMessage(string message, MessageType messageType)
        {
            if (_throwMessage != null)
            {
                _throwMessage(message, messageType);
            }
        }

        #endregion

        #region Abstract methods

        //make abstract this interface method so that subclass can implement it
        public abstract void ProcessLogList();

        protected abstract bool Register(ActiveUserResponse member, string sPassword, int iPrivilege);

        protected abstract bool UnRegister(LoginResponse member, string sPassword, int iPrivilege);

        #endregion
    }
}
