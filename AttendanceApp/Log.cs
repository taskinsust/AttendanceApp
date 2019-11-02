using AttendanceApp.Models;
using AttendanceApp.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using zkemkeeper;
using Timer = System.Windows.Forms.Timer;

namespace AttendanceApp
{
    public partial class Log : Form
    {
        public CZKEM _czkem { get; set; }
        public int _count { get; set; } = 0;
        public DeviceTypes _device { get; set; }
        public DateTime fetchTime { get; set; } = new DateTime(1999, 1, 1, 0, 0, 0);
        public DataTable tblS { get; set; } = new DataTable();
        public Timer myTimer { get; set; } = new Timer();

        public Log()
        {
            InitializeComponent();
        }

        private void Log_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitDevices();
            OnVerifyThumb();
            DefineTblsFields();
            //RefreshUserAndData();
            refreshUsersAndData.Start();

            myTimer.Interval = 10 * 1000;
            myTimer.Start();
            myTimer.Tick += new EventHandler(MyTimer_Tick);

        }

        #region Form Load Methods
        private void InitDevices()
        {
            //This should come from server side.
            _device = new DeviceTypes
            {
                DeviceName = "1",
                DeviceIp = ConfigurationManager.AppSettings["DeviceIPIN"].ToString(),
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["Deviceport"].ToString()),
                DeviceType = "IN",
                BranchId = -1,
                DeviceId = 1,
                CommPassword = 123456
            };
        }

        private void OnVerifyThumb()
        {
            var thread = new Thread(o => GlobalHandler(() =>
           {
               var _czkem1 = new CZKEM();
               if (!_device.IsConDevice)
               {

                   var connect = _czkem1.Connect_Net(_device.DeviceIp, _device.Port);
                   if (connect)
                       _device.IsConDevice = true;
               }
               if (_device.IsConDevice)
               {
                   if (_czkem1.RegEvent(1, 65535))
                   {
                       _czkem1.OnAttTransactionEx += new _IZKEMEvents_OnAttTransactionExEventHandler(zkemClient_OnAttTransactionEx);
                   }
               }
               Application.Run();
           }));
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void DefineTblsFields()
        {
            tblS.Columns.Add("intEmployeeId");
            tblS.Columns.Add("dtDate");
            tblS.Columns.Add("intInOut");
            tblS.Columns.Add("intBranchId");
        }

        private void StartTimers()
        {
            refreshUsersAndData.Start();
        }

        #endregion

        #region Button Click functions

        private void btnActive_Click(object sender, EventArgs e)
        {
            ActiveUser();
        }

        private void btnDeActive_Click(object sender, EventArgs e)
        {
            InActiveUser();
        }

        private void btnSyncAttendance_Click(object sender, EventArgs e)
        {
            SyncAttendance();
        }

        private void btnDeleteAttendanceLog_Click(object sender, EventArgs e)
        {
            var answer = MessageBox.Show("Are you sure to delete all attendance log?", "Yes/no sample", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer == DialogResult.Yes)
            {
                var thread = new Thread(() =>
                {
                    _czkem = new CZKEM();
                    var connect = _czkem.Connect_Net(_device.DeviceIp, _device.Port);
                    if (connect)
                        _device.IsConDevice = true;
                    if (_device.IsConDevice)
                    {
                        _czkem.ClearGLog(1);
                        _czkem.RefreshData(1);
                        fillListView("Attendance Log Deleted from Device Sucessfully", 0);
                    }
                    //Application.Run();
                });
                thread.IsBackground = true;
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Settings.Default.CompanyId = -1;
            Settings.Default.BranchId = -1;
            Settings.Default.Mode = "nutural";
            Settings.Default.Save();
            Application.Exit();
        }

        #endregion

        #region Timer Tick Events

        private void refreshUsersAndData_Tick(object sender, EventArgs e)
        {
            RefreshUserAndData();
        }

        private void MyTimer_Tick(object sender, System.EventArgs e)
        {
            ActiveUser();
            InActiveUser();
            myTimer.Stop();
        }

        private void RefreshUserAndData()
        {
            ActiveUser();
            InActiveUser();
            SyncAttendance();
        }

        #endregion


        #region Click Operations

        private async void ActiveUser()
        {
            btnActive.Enabled = false;
            bool isEnabled = true;
            bool returnResult = false, isExitToDevice = false;
            //  var thread = new Thread(async () =>
            //{
            try
            {
                var obj = new Dictionary<string, string>();
                obj.Add("gymid", Settings.Default.BranchId.ToString());
                var data = JsonConvert.SerializeObject(obj);
                var url = ConfigurationManager.AppSettings["ApiUrl"].ToString() + "webappservices/getbackupactivemembers";
                var queryString = new StringContent(data, Encoding.UTF8, "text/plain");
                using (var client = new HttpClient())
                {
                    //var result = await client.PostAsync(new Uri(url), queryString);
                    //string resultContent = await result.Content.ReadAsStringAsync();
                    // var res = JsonConvert.DeserializeObject<ActiveUserResponse[]>(resultContent);

                    //For Debug Purpose Only
                    string resultContent = ServerResponseForActiveUserResponseModel();
                    var res = JsonConvert.DeserializeObject<List<ActiveUserResponse>>(resultContent);
                    if (res != null && res.Any())
                    {
                        _czkem = new CZKEM();
                        _czkem.SetCommPassword(_device.CommPassword);
                        var connect = _czkem.Connect_Net(_device.DeviceIp, _device.Port);
                        if (connect)
                            _device.IsConDevice = true;
                        if (_device.IsConDevice)
                        {
                            await fillListView("Device connected Successfully.", 0);
                            foreach (var item in res)
                            {
                                //User registered using both Card and Thumb
                                if (!String.IsNullOrEmpty(item.cardNumber))
                                {
                                    bool isSet = _czkem.SetStrCardNumber(item.cardNumber);
                                    if (isSet)
                                    {
                                        if (isEnabled)
                                            returnResult = _czkem.SSR_SetUserInfo(1, item.member_id, item.userName, "", 0, isEnabled);
                                    }
                                }
                                if (_czkem.SSR_SetUserInfo(1, item.member_id, "", "", 0, true))
                                {
                                    var enable = _czkem.SetUserTmpExStr(1, item.member_id, item.fingerIndex, 1, item.templateData);
                                }
                            }
                            await fillListView("Active Users Done Successfully.", 0);
                        }
                    }
                    else
                    {
                        await fillListView("No record found to Active users.", 0);
                    }
                }
            }
            catch (Exception)
            {

            }
            btnActive.Enabled = true;
            //});
            // thread.IsBackground = true;
            // thread.SetApartmentState(ApartmentState.STA);
            // thread.Start();
        }

        private void InActiveUser()
        {
            btnDeActive.Enabled = false;
            var thread = new Thread(async () =>
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
                    var res = JsonConvert.DeserializeObject<LoginResponse[]>(resultContent);
                    if (res != null && res.Any())
                    {
                        _czkem = new CZKEM();
                        var connect = _czkem.Connect_Net(_device.DeviceIp, _device.Port);
                        if (connect)
                            _device.IsConDevice = true;
                        if (_device.IsConDevice)
                        {
                            _czkem.ReadAllTemplate(_device.DeviceId);

                            string sTmpData = string.Empty;
                            int iTmpLength = 0, iFlag = 0;

                            foreach (var item in res)
                            {
                                for (int i = 0; i <= 9; i++)
                                {
                                    if (_czkem.GetUserTmpExStr(_device.DeviceId, item.id, i, out iFlag, out sTmpData, out iTmpLength))
                                    {
                                        var sendData = new SaveApiTemplate
                                        {
                                            gymid = Settings.Default.BranchId,
                                            BackUpTemplate = new List<BackUpTemplate>
                                            {
                                                new BackUpTemplate
                                                {
                                                    name = "",
                                                    member_id = item.id,
                                                    Password = "",
                                                    prvlg = 1,
                                                    enabled = 1,
                                                    fingerIndex = i,
                                                    flag = iFlag,
                                                    templateData = sTmpData

                                                }
                                            }
                                        };
                                        SendTemplateBackUpToAPI(sendData);
                                        _czkem.SSR_DelUserTmp(_device.DeviceId, item.id, i);
                                    }
                                }
                            }
                        }

                        await fillListView("In-Active Users Done Successfully", 0);
                    }
                    else
                    {
                        await fillListView("No record found to In-Active users", 0);
                    }
                }
                btnDeActive.Enabled = true;
            });
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void SyncAttendance()
        {
            btnSyncAttendance.Enabled = false;
            //  var thread = new Thread(async () => {
            tblS.Clear();
            var dates = new List<DateTime>();
            var strResetTime = string.Empty;
            var iGLCount = 0;
            _czkem = new CZKEM();
            var connect = _czkem.Connect_Net(_device.DeviceIp, _device.Port);
            if (connect)
                _device.IsConDevice = true;
            if (_device.IsConDevice)
            {
                var readLog = _czkem.ReadAllGLogData(1);
                if (readLog)
                {
                    //int idwErrorCode = 0;
                    string sdwEnrollNumber = "";
                    var idwVerifyMode = 0;
                    var idwInOutMode = 0;
                    var idwYear = 0;
                    var idwMonth = 0;
                    var idwDay = 0;
                    var idwHour = 0;
                    var idwMinute = 0;
                    var idwSecond = 0;
                    var idwWorkcode = 0;
                    while (_czkem.SSR_GetGeneralLogData(_device.DeviceId, out sdwEnrollNumber, out idwVerifyMode,
                                 out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour,
                                 out idwMinute, out idwSecond, ref idwWorkcode))
                    {
                        double memberId;
                        if (Double.TryParse(sdwEnrollNumber, out memberId))
                        {
                            iGLCount++;
                            var date = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond);
                            var dr = tblS.NewRow();
                            dr["intEmployeeId"] = sdwEnrollNumber;
                            dr["dtDate"] = date.ToString();
                            dr["intInOut"] = idwVerifyMode;
                            dr["intBranchId"] = _device.BranchId;
                            if (date >= Settings.Default.Date)
                            {
                                if (string.IsNullOrEmpty(strResetTime))
                                {
                                    strResetTime = date.ToString();
                                }
                                tblS.Rows.Add(dr);
                                dates.Add(date);
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(strResetTime))
                    {
                        strResetTime = Settings.Default.Date.ToString();
                    }
                    if (dates.Any())
                    {
                        Settings.Default.Date = dates.OrderByDescending(x => x).First();
                    }
                    //await fillListView("Starting from " + strResetTime + " Compare To " + Settings.Default.Date, 0);
                    //fillListView("fetched In", iGLCount);
                    SaveSyncData(tblS);
                }
                else
                {
                    //await fillListView("No data found from device to sync.", 0);
                    btnSyncAttendance.Enabled = true;
                }
            }
            // });
            // thread.IsBackground = true;
            // thread.SetApartmentState(ApartmentState.STA);
            // thread.Start();
        }

        #endregion


        #region Local Functions

        private async Task fillListView(string act, int total, string ex2 = "")
        {
            var thread = new Thread((object val) =>
            {
                var param = (FillListViewModel)val;
                var item1 = new ListViewItem();
                _count++;
                var imgList = new ImageList();
                imgList.Images.Add(Resources.success);
                imgList.Images.Add(Resources.error);
                gridLogDetailsView.SmallImageList = imgList;
                if (param.act == "error")
                {
                    item1.Text = "Error";
                    item1.SubItems.Add(_count.ToString());
                    item1.SubItems.Add("Error Occured");
                    item1.SubItems.Add(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt"));
                    item1.SubItems.Add(param.ex2);
                    item1.ImageIndex = 1;
                }
                else
                {
                    item1.Text = "Success";
                    item1.SubItems.Add(_count.ToString());
                    item1.SubItems.Add(param.act);
                    item1.SubItems.Add(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt"));
                    item1.SubItems.Add(param.ex2);
                    item1.ImageIndex = 0;
                }
                gridLogDetailsView.Items.Insert(0, item1);
                //Application.Run();
            });
            var obj = new FillListViewModel
            {
                act = act,
                ex2 = ex2,
                total = total
            };
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(obj);
        }

        #endregion

        #region Api Call To Save User data

        private async void SendTemplateBackUpToAPI(SaveApiTemplate backdata)
        {
            var url = ConfigurationManager.AppSettings["ApiUrl"].ToString() + "webappservices/SetBackup";
            var data = JsonConvert.SerializeObject(backdata);
            var queryString = new StringContent(data, Encoding.UTF8, "text/plain");
            using (var client = new HttpClient())
            {
                var result = await client.PostAsync(new Uri(url), queryString);
                string resultContent = await result.Content.ReadAsStringAsync();
            }
        }

        private void SaveSyncData(DataTable tblS)
        {
            var thread = new Thread(async () =>
            {
                var tempEmployeeId = -1;
                var saveCount = 0;
                if (tblS != null)
                {
                    DateTime dtTempFetch = fetchTime;
                    var dataView = new DataView(tblS);
                    dataView.Sort = " dtDate ASC ";
                    var dtFinal = dataView.ToTable();
                    if (dtFinal.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtFinal.Rows.Count - 1; i++)
                        {
                            var row = dtFinal.Rows[i];
                            tempEmployeeId = Convert.ToInt32(row["intEmployeeId"]);
                            var atDate = Convert.ToDateTime(row["dtDate"]);
                            var branchId = Convert.ToInt32(row["intBranchId"]);
                            if (fetchTime < atDate)
                            {
                                var sendData = new SaveAttendanceModel
                                {
                                    gymid = Settings.Default.CompanyId,
                                    attendences = new List<Attendences>
                            {
                                new Attendences
                                {
                                    date = Convert.ToDateTime(atDate).ToString("yyyy-MM-dd"),
                                    member_id = tempEmployeeId,
                                    time = atDate.ToString("HH:mm:ss")
                                }
                            }
                                };
                                var data = JsonConvert.SerializeObject(sendData);
                                var url = ConfigurationManager.AppSettings["ApiUrl"].ToString() + "webappservices/addattendences";
                                var queryString = new StringContent(data, Encoding.UTF8, "text/plain");
                                using (var client = new HttpClient())
                                {
                                    var result = await client.PostAsync(new Uri(url), queryString);
                                    string resultContent = await result.Content.ReadAsStringAsync();
                                    saveCount++;
                                    if (Convert.ToDateTime(atDate) > dtTempFetch)
                                    {

                                        dtTempFetch = atDate;
                                    }
                                    Settings.Default.Date = atDate;
                                    Settings.Default.Save();
                                }
                            }
                        }
                        fetchTime = dtTempFetch;
                        if (saveCount > 0)
                        {
                            await fillListView("Attendance saved to the server successfully.", saveCount);
                        }
                    }
                    else
                    {
                        await fillListView("No record to perforn sync", 0);
                    }
                }
                else
                {
                    await fillListView("No record to perforn sync", 0);
                }
                btnSyncAttendance.Enabled = true;
                //Application.Run();
            });
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        #endregion

        private async void zkemClient_OnAttTransactionEx(string EnrollNumber, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second, int WorkCode)
        {
            var gymId = Settings.Default.BranchId;

            var obj = new Dictionary<string, string>();
            obj.Add("gymid", Settings.Default.BranchId.ToString());
            obj.Add("memberid", EnrollNumber);
            //var data = JsonConvert.SerializeObject(obj);
            var url = ConfigurationManager.AppSettings["ApiUrl"].ToString() + "webappservices/meberdetail";
            //var queryString = new StringContent(data, Encoding.UTF8, @"application/x-www-form-urlencoded");
            using (var client = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(obj) };
                var result = await client.SendAsync(req);
                //var result = await client.PostAsync(new Uri(url), queryString);
                string resultContent = await result.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<ApiResponseModel>(resultContent);
                if (res != null && res.memberdetail != null && !string.IsNullOrEmpty(res.memberdetail.name))
                {
                    var attendance = new AttendanceModel
                    {
                        Fee = res.memberdetail.fees,
                        FeeDate = res.memberdetail.fee_date,
                        JoinDate = res.memberdetail.joining_date,
                        Name = res.memberdetail.name,
                        Phone = res.memberdetail.phone,
                        Seconds = res.memberdetail.seconds != null && res.memberdetail.seconds > 0 ? Convert.ToInt32(Second) : 30
                    };
                    OpenPopUp(attendance);
                }
            }
        }

        private void OpenPopUp(AttendanceModel attendance)
        {
            var thread = new Thread(o => GlobalHandler(() =>
            {
                var frmBuild = new Popup(attendance);
                Application.Run(frmBuild);
            }));
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }


        public static void GlobalHandler(ThreadStart threadStartTarget)
        {
            try
            {
                threadStartTarget.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var attendance = new AttendanceModel
            {
                Fee = "120",
                FeeDate = "11-11-1111",
                JoinDate = "12-12-1212",
                Name = "Usman",
                Phone = "111-111-111",
                Seconds = 10
            };
            OpenPopUp(attendance);
        }


        #region Debug

        private string ServerResponseForActiveUserResponseModel()
        {
            var response = new List<ActiveUserResponse>() {
                new ActiveUserResponse()
                {
                    cardNumber="4348805026284589",
                    member_id="1"
                }
            };
            //var response = new ActiveUserResponse()
            //{
            //    cardNumber = "4348805026284589",
            //    member_id = "1"

            //};

            var result = JsonConvert.SerializeObject(response);
            return result.ToString();
        }
        #endregion
    }
}
