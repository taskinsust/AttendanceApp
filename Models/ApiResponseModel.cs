using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.Models
{
    public class ApiResponseModel
    {
        public MemberDetail memberdetail { get; set; }
    }
    public class MemberDetail
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string gender { get; set; }
        public string blood_group { get; set; }
        public string body_weight { get; set; }
        public string height { get; set; }
        public string secondary_name { get; set; }
        public string secondary_phone { get; set; }
        public string comment { get; set; }
        public string refrence_no { get; set; }
        public string cnic { get; set; }
        public string sms { get; set; }
        public string address { get; set; }
        public string package { get; set; }
        public string image { get; set; }
        public string fees { get; set; }
        public string packagename { get; set; }
        public string duration { get; set; }
        public string joining_date { get; set; }
        public string fee_date { get; set; }
        public string status { get; set; }
        public int? seconds { get; set; }

    }
}
