using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.Models
{
    public class ActiveUserResponse
    {
        public string member_id { get; set; }
        public int fingerIndex { get; set; }
        public string templateData { get; set; }
    }
}
