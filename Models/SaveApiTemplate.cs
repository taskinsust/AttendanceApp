using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.Models
{
    public class SaveApiTemplate
    {
        public int gymid { get; set; }
        public List<BackUpTemplate> BackUpTemplate { get; set; }
    }
    public class BackUpTemplate
    {
        public string name { get; set; }
        public string member_id { get; set; }
        public string Password { get; set; }
        public int prvlg { get; set; }
        public int enabled { get; set; }
        public int fingerIndex { get; set; }
        public int flag { get; set; }
        public string templateData { get; set; }
    }

}
