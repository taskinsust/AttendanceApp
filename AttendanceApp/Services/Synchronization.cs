using AttendanceApp.DeviceHelper;
using AttendanceApp.Helper;
using AttendanceApp.Models;
using AttendanceApp.Properties;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using zkemkeeper;

namespace AttendanceApp.Services
{
    public class Synchronization
    {
        private Thread _deviceSynchronizerThread;
        private Thread _inactiveThread;
        private Thread _SyncThread;
        private Thread _deleteDataThread;

        IAttendanceDeviceDriver attendanceDeviceDriver = null;
        public volatile ServiceStatus _serviceStatus;
        public volatile SyncStatus _syncStatus;

        public volatile bool InternetAvailable = false;
        private const int ThreadJoiningTime = 10 * 1000;
        private int _serverCallingTimeInterval = 30;
        private const int MinuteToSec = 60;
        private const int TimeInterval = 10;

        private readonly ServiceDelegate.DeviceSynchronizedEventHandler _throwMessage;
        private AttendanceDeviceModel _attendanceDeviceModel;
        private static readonly ILog serviceLog = LogManager.GetLogger("ServiceLogger");

        public Synchronization(ServiceDelegate.DeviceSynchronizedEventHandler throwMessage, AttendanceDeviceModel attendanceDeviceModel)
        {
            _serviceStatus = new ServiceStatus();
            _syncStatus = new SyncStatus();
            _attendanceDeviceModel = attendanceDeviceModel;
            _throwMessage = throwMessage;
        }

        #region Delegate Function

        protected virtual void OnThrowingMessage(string message, MessageType messageType)
        {
            if (_throwMessage != null)
            {
                _throwMessage(message, messageType);
            }
        }

        #endregion

        #region Thread Start and Stop Point

        public void Start()
        {
            try
            {
                if (_deviceSynchronizerThread != null && _deviceSynchronizerThread.IsAlive)
                {
                    OnThrowingMessage("Program already running Please stop it first", MessageType.Info);
                }
                else
                {
                    _syncStatus.IsRunning = true;
                    _deviceSynchronizerThread = new System.Threading.Thread(DeviceSync);
                    _deviceSynchronizerThread.SetApartmentState(ApartmentState.STA);
                    _deviceSynchronizerThread.Start();
                }
            }

            catch (Exception ex)
            {
                serviceLog.Error("Could not start ", ex);
                OnThrowingMessage("Service fails to start due to " + ex.Message, MessageType.Error);
            }
        }

        public void Stop()
        {
            try
            {
                _serviceStatus.IsRunning = false;

                _deviceSynchronizerThread.Join();
                _inactiveThread.Join();
                _SyncThread.Join();
                _deleteDataThread.Join();
            }
            catch (ThreadStartException ese)
            {
                throw ese;
            }
            catch (ThreadInterruptedException tie)
            {
                throw tie;
            }
            catch (Exception ex)
            {
                serviceLog.Error("stop failed due to " + ex.ToString());
                //OnThrowingMessage("Service fails to stop due to " + ex.Message, MessageType.Error);
            }
        }

        public void Restart()
        {
            try
            {
                _serviceStatus.IsRunning = false;
                _syncStatus.IsRunning = false;

                //if (_deviceSynchronizerThread.IsAlive)
                //    _deviceSynchronizerThread.Join();
                //if (_inactiveThread.IsAlive)
                //    _inactiveThread.Join();
                //if (_SyncThread.IsAlive)
                //    _SyncThread.Join();
                //if (_deleteDataThread.IsAlive)
                //    _deleteDataThread.Join();
                Start();
            }
            catch (Exception ex)
            {
                serviceLog.Error("Restart failed due to " + ex.Message);
                OnThrowingMessage("Service fails to restart due to " + ex.Message, MessageType.Error);
            }
        }

        public void StartInActiveUser()
        {
            try
            {
                _serviceStatus.IsRunning = false;
                try
                {
                    //if (_deviceSynchronizerThread.IsAlive)
                    //    _deviceSynchronizerThread.Join();
                    if (_SyncThread.IsAlive)
                        _SyncThread.Join();
                    if (_deleteDataThread.IsAlive)
                        _deleteDataThread.Join();
                }
                catch
                {
                }
                finally
                {
                    //_deviceSynchronizerThread = null;
                    _SyncThread = null;
                    _deleteDataThread = null;
                }

                Thread.Sleep(2000);

                _serviceStatus.IsRunning = true;
                _inactiveThread = new System.Threading.Thread(InActiveUser);
                _inactiveThread.SetApartmentState(ApartmentState.STA);
                _inactiveThread.Start();
            }

            catch (Exception ex)
            {
                serviceLog.Error("Could not start ", ex);
                OnThrowingMessage("Service fails to start due to " + ex.Message, MessageType.Error);
            }
        }

        internal void StartSyncAttendance()
        {
            try
            {
                _serviceStatus.IsRunning = false;
                try
                {
                    //if (_deviceSynchronizerThread.IsAlive)
                    //    _deviceSynchronizerThread.Join();
                    if (_inactiveThread.IsAlive)
                        _inactiveThread.Join();
                    if (_deleteDataThread.IsAlive)
                        _deleteDataThread.Join();
                }
                catch
                {
                }
                finally
                {
                    //_deviceSynchronizerThread = null;
                    _inactiveThread = null;
                    _deleteDataThread = null;
                }

                _serviceStatus.IsRunning = true;
                _SyncThread = new System.Threading.Thread(SyncAttendance);
                _SyncThread.SetApartmentState(ApartmentState.STA);
                _SyncThread.Start();

            }

            catch (Exception ex)
            {
                serviceLog.Error("Could not start ", ex);
                OnThrowingMessage("Service fails to start due to " + ex.Message, MessageType.Error);
            }
        }

        private void SafeSleep(int second)
        {
            int currentSec = 0;
            while (_syncStatus.IsRunning && currentSec < second)
            {
                currentSec++;
                Thread.Sleep(1000);
            }
        }

        #endregion

        #region Operation

        [STAThread]
        private async void DeviceSync()
        {
            //starting thread
            string info = "Service Started at: " + DateTime.Now.ToString(BusinessRules.DateTimeFormat);

            OnThrowingMessage(info, MessageType.Important);

            while (_syncStatus.IsRunning)
            {
                try
                {
                    //get data from online
                    OnThrowingMessage("Try to get active member from online", MessageType.Info);

                    var obj = new Dictionary<string, string>();
                    obj.Add("gymid", Settings.Default.BranchId.ToString());
                    var data = JsonConvert.SerializeObject(obj);
                    var url = ConfigurationManager.AppSettings["ApiUrl"].ToString() + "webappservices/getbackupactivemembers";
                    var queryString = new StringContent(data, Encoding.UTF8, "text/plain");
                    try
                    {

                        List<ActiveUserResponse> response = null;
                        using (var client = new HttpClient())
                        {
                            var result = await client.PostAsync(new Uri(url), queryString);
                            string resultContent = await result.Content.ReadAsStringAsync();
                            response = JsonConvert.DeserializeObject<ActiveUserResponse[]>(resultContent).ToList();

                        }

                        if (response == null)
                        {
                            OnThrowingMessage("Failed to get data from online", MessageType.Error);
                        }
                        else
                        {
                            _serverCallingTimeInterval = MinuteToSec * 5;
                            _attendanceDeviceModel.TeamMembers = response;
                            SyncOperation(_attendanceDeviceModel);

                        }
                    }
                    catch (Exception e)
                    {
                        OnThrowingMessage("Api Not responding properly!" + url, MessageType.Error);
                    }


                    OnThrowingMessage("Next Sync Time: " + DateTime.Now.AddSeconds(_serverCallingTimeInterval).ToString(BusinessRules.DateTimeFormat), MessageType.Important);
                    SafeSleep(_serverCallingTimeInterval);
                }
                catch (WebException webException)
                {
                    serviceLog.Error(webException);
                    OnThrowingMessage("Internet unavailable", MessageType.Error);
                    SafeSleep(TimeInterval);
                }
                catch (Exception exception)
                {
                    serviceLog.Error(exception);
                    SafeSleep(TimeInterval);
                }
            }

            //stoping thread
            // info = "Service Stopped at: " + DateTime.Now.ToString(BusinessRules.DateTimeFormat);

            //OnThrowingMessage(info, MessageType.Important); Console.WriteLine("after execution !");
        }

        private void SyncOperation(AttendanceDeviceModel attendanceDeviceModel)
        {
            IAttendanceDeviceDriver attendanceDeviceDriver = null;
            try
            {
                attendanceDeviceDriver = AttendanceDeviceDriverFactory(ConfigurationManager.AppSettings["deviceTypeCode"].ToString());
                if (attendanceDeviceDriver == null || attendanceDeviceDriver.OpenConnection() == false)
                    return;

                #region Device Management

                var deviceManagement = new DeviceManagement(attendanceDeviceModel, attendanceDeviceDriver);
                bool isSuccess = deviceManagement.ReadyDevice();
                if (!isSuccess) return;

                #endregion

                #region UserManagement

                var userManagement = new UserManagement(attendanceDeviceModel, attendanceDeviceDriver, _serviceStatus, OnThrowingMessage);
                bool isSuc = userManagement.RegisterUser();

                if (!isSuc) return;

                #endregion

                #region Attendance Synchronization

                var attendanceManagement = new AttendanceManagement(attendanceDeviceModel, attendanceDeviceDriver, OnThrowingMessage);
                attendanceManagement.UpdateAttendance();

                #endregion

                #region Update Time

                OnThrowingMessage("Sync completed on device " + attendanceDeviceModel.IPAddress + " ", MessageType.Info);

                #endregion

            }
            catch (Exception e) { }
        }

        [STAThread]
        private async void InActiveUser()
        {
            attendanceDeviceDriver = AttendanceDeviceDriverFactory(ConfigurationManager.AppSettings["deviceTypeCode"].ToString());
            if (attendanceDeviceDriver == null || attendanceDeviceDriver.OpenConnection() == false)
                return;

            while (_serviceStatus.IsRunning)
            {
                var obj = new Dictionary<string, string>();
                obj.Add("gymid", Settings.Default.BranchId.ToString());
                var data = JsonConvert.SerializeObject(obj);
                var url = ConfigurationManager.AppSettings["ApiUrl"].ToString() + "webappservices/GetInactiveMembers";
                var queryString = new StringContent(data, Encoding.UTF8, "text/plain");

                using (var client = new HttpClient())
                {
                    var result = await client.PostAsync(new Uri(url), queryString);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<LoginResponse[]>(resultContent).ToList();
                    _attendanceDeviceModel.LoginResponse = res;

                    if (res != null && res.Any())
                    {
                        var userManagement = new UserManagement(_attendanceDeviceModel, attendanceDeviceDriver, _serviceStatus, OnThrowingMessage);
                        bool isSuc = userManagement.UnRegisterUser();
                        OnThrowingMessage("In-Active Users Done Successfully", MessageType.Info);
                    }
                    else
                    {
                        OnThrowingMessage("No record found to In-Active users", MessageType.Info);
                    }
                }
                _serviceStatus.IsRunning = false;
            }
        }

        [STAThread]
        private void SyncAttendance()
        {
            attendanceDeviceDriver = AttendanceDeviceDriverFactory(ConfigurationManager.AppSettings["deviceTypeCode"].ToString());
            if (attendanceDeviceDriver == null || attendanceDeviceDriver.OpenConnection() == false)
                return;

            while (_serviceStatus.IsRunning)
            {
                var attendanceManagement = new AttendanceManagement(_attendanceDeviceModel, attendanceDeviceDriver, OnThrowingMessage);
                attendanceManagement.UpdateAttendance();
                _serviceStatus.IsRunning = false;
            }
        }

        public IAttendanceDeviceDriver AttendanceDeviceDriverFactory(string deviceType)
        {
            if (deviceType.Equals(DeviceType.ColoredDevice, StringComparison.CurrentCultureIgnoreCase))
            {
                return new AxAttendanceDeviceDriverColor(_attendanceDeviceModel, OnThrowingMessage);
            }
            else if (deviceType.Equals(DeviceType.ColoredDeviceWithFingerTemplate, StringComparison.CurrentCultureIgnoreCase))
            {
                return new AxAttendanceDeviceDriverColorWithFinger(_attendanceDeviceModel, OnThrowingMessage);
            }
            else
            {
                return null;
            }
        }

        #endregion
    }

    public class ServiceStatus
    {
        public volatile bool IsRunning = false;
    }
    public class SyncStatus
    {
        public volatile bool IsRunning = false;
    }
}
