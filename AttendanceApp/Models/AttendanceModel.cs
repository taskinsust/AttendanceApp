using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.Models
{
    public class AttendanceModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string JoinDate { get; set; }
        public string FeeDate { get; set; }
        public string Fee { get; set; }
        public int Seconds { get; set; }
    }
}
