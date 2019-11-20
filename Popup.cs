using AttendanceApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AttendanceApp
{
    public partial class Popup : Form
    {
        public Popup(AttendanceModel attendance)
        {
            InitializeComponent();
            lblFee.Parent = PictureBox3;
            Label5.Parent = PictureBox3;

            lblFeeDate.Parent = PictureBox4;

            lblFee.Text = "RS " + attendance.Fee;
            lblFeeDate.Text = attendance.FeeDate;
            lblJoinDate.Text = attendance.JoinDate;
            lblPhone.Text = attendance.Phone;
            lblName.Text = attendance.Name;

            var Timer1 = new Timer();
            Timer1.Interval = attendance.Seconds > 0 ? attendance.Seconds * 1000 : 15 * 1000;
            Timer1.Start();
            Timer1.Tick += new EventHandler(Timer1_Tick);
            
        }
        
        private void Timer1_Tick(object sender, System.EventArgs e)
        {
            this.Close();
        }


    }
}
