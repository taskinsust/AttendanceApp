using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.Models
{
    public class AttendanceLog
    {
        public virtual int ID { get; set; }
        public virtual int Pin { get; set; }
        public virtual int PunchType { get; set; }
        public virtual string PunchTime { get; set; }
        public virtual int DeviceId { get; set; }
        public virtual int EnrollNumber { get; set; }   //DTP
    }
}
