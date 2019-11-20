using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.Models
{
    public class SaveAttendanceModel
    {
        public int gymid { get; set; }
        public List<Attendences> attendences { get; set; }
    }
    public class Attendences
    {
        public string date { get; set; }
        public int member_id { get; set; }
        public string time { get; set; }

    }
}
