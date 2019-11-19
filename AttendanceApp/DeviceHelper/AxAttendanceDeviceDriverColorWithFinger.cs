using AttendanceApp.Helper;
using AttendanceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.DeviceHelper
{
    public class AxAttendanceDeviceDriverColorWithFinger : AxAttendanceDeviceDriverColor
    {
        public AxAttendanceDeviceDriverColorWithFinger(AttendanceDeviceModel attendanceDevice, MessagePass throwMessage)
            : base(attendanceDevice, throwMessage)
        {
        }

        protected override bool Register(ActiveUserResponse member, string sPassword, int iPrivilege)
        {
            bool returnResult = false;
            try
            {
                returnResult = base.Register(member, sPassword, iPrivilege);

                if (returnResult)
                {
                    for (int fingerIndex = 0; fingerIndex < 10; fingerIndex++)
                    {
                        if (fingerIndex == member.fingerIndex)
                        {
                            //remove & update finger 
                            AxDevice.SSR_DelUserTmpExt(_attendanceDevice.MachineNo, member.member_id.ToString(), fingerIndex);
                            var isSuccess = AxDevice.SetUserTmpExStr(_attendanceDevice.MachineNo, member.member_id, member.fingerIndex, 1, member.templateData);
                            if (isSuccess)
                                OnThrowingMessage("Register user " + member.userName + " in device with Finger " + member.member_id, MessageType.Info);
                        }
                    }
                }
                return returnResult;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override void ProcessLogList()
        {
            base.ProcessLogList();
        }

        protected override bool UnRegister(LoginResponse member, string sPassword, int iPrivilege)
        {
            return base.UnRegister(member, sPassword, iPrivilege);
        }
    }
}
