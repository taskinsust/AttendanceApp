using AttendanceApp.Helper;
using AttendanceApp.Models;
using AttendanceApp.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.DeviceHelper
{
    public class AxAttendanceDeviceDriverColor : AxAttendanceDeviceDriver
    {
        public DataTable tblS { get; set; } = new DataTable();
        public DateTime fetchTime { get; set; } = new DateTime(1999, 1, 1, 0, 0, 0);

        public AxAttendanceDeviceDriverColor(AttendanceDeviceModel attendanceDevice, MessagePass throwMessage)
            : base(attendanceDevice, throwMessage)
        {
        }

        protected override bool Register(ActiveUserResponse member, string sPassword, int iPrivilege)
        {
            bool returnResult = false, isExitToDevice = false;
            string name, password;
            int priv;
            try
            {
                bool isEnabled = true;
                if (!String.IsNullOrEmpty(member.cardNumber))
                {
                    bool isSet = AxDevice.SetStrCardNumber(member.cardNumber);
                    if (!isSet) return false;
                    if (isEnabled)
                        returnResult = AxDevice.SSR_SetUserInfo(_attendanceDevice.MachineNo, member.member_id.ToString(), member.userName, sPassword, iPrivilege, isEnabled);
                    else
                    {
                        //check if user exits in device. Delete User if only Exit
                        AxDevice.SSR_GetUserInfo(_attendanceDevice.MachineNo, member.member_id.ToString(), out name, out password, out priv, out isExitToDevice);
                        if (isExitToDevice)
                            returnResult = AxDevice.SSR_DeleteEnrollData(_attendanceDevice.MachineNo, member.member_id.ToString(), BusinessRules.BackupNumber);
                        else
                            return returnResult;
                    }

                    if (returnResult)
                        OnThrowingMessage("Register user " + member.userName + " in device with id " + member.member_id, MessageType.Info);
                    else
                        OnThrowingMessage("Failed to save user " + member.userName + " in device ", MessageType.Error);
                }
                return returnResult;
            }
            catch (Exception ex)
            {
                //Log.Error(ex);
                return false;
            }
        }

        private void DefineTblsFields()
        {
            tblS.Columns.Add("intEmployeeId");
            tblS.Columns.Add("dtDate");
            tblS.Columns.Add("intInOut");
            tblS.Columns.Add("intBranchId");
        }

        public override void ProcessLogList()
        {
            //for color device
            var idwEnrollNumber = "";
            var idwVerifyMode = 0;
            var idwInOutMode = 0;
            var idwYear = 0;
            var idwMonth = 0;
            var idwDay = 0;
            var idwHour = 0;
            var idwMinute = 0;
            var idwSecond = 0;
            var idwWorkCode = 0;
            var strResetTime = string.Empty;
            var dates = new List<DateTime>();
            var attdlogList = new List<AttendanceLog>();
            try
            {
                OnThrowingMessage("Retriving attendance data from device", MessageType.Info);
                DefineTblsFields();
                //for color
                while (AxDevice.SSR_GetGeneralLogData(_attendanceDevice.MachineNo, out idwEnrollNumber, out idwVerifyMode, out idwInOutMode,
                    out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkCode))
                {
                    double memberId;
                    if (Double.TryParse(idwEnrollNumber, out memberId))
                    {
                        // iGLCount++;
                        var date = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond);
                        

                        var dr = tblS.NewRow();
                        dr["intEmployeeId"] = idwEnrollNumber;
                        dr["dtDate"] = date.ToString();
                        dr["intInOut"] = idwVerifyMode;
                        dr["intBranchId"] = _attendanceDevice.BranchId;
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
            }
            catch (Exception ex)
            {
                // Log.Error(ex);
                throw;
            }
        }

        protected override bool UnRegister(LoginResponse item, string sPassword, int iPrivilege)
        {
            try
            {
                for (int i = 0; i <= 9; i++)
                {
                    string sTmpData = string.Empty;
                    int iTmpLength = 0, iFlag = 0;

                    if (AxDevice.GetUserTmpExStr(_attendanceDevice.Id, item.id, i, out iFlag, out sTmpData, out iTmpLength))
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
                        AxDevice.SSR_DelUserTmp(_attendanceDevice.Id, item.id, i);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async void SaveSyncData(DataTable tblS)
        {
            if (tblS != null)
            {
                var tempEmployeeId = -1;
                var saveCount = 0;
                DateTime dtTempFetch = fetchTime;
                var dataView = new DataView(tblS);
                dataView.Sort = " dtDate ASC ";
                var dtFinal = dataView.ToTable();
                if (dtFinal.Rows.Count > 0)
                {
                    for (int i = 0; i <= dtFinal.Rows.Count - 1; i++)
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
                        OnThrowingMessage("Attendance saved to the server successfully " + saveCount, MessageType.Info);
                    }
                }
                else
                {
                    OnThrowingMessage("No record to perforn sync " + saveCount, MessageType.Info);
                }
            }
        }

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
    }
}

