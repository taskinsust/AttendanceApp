using AttendanceApp.Models;
using AttendanceApp.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

namespace AttendanceApp
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            var thread = new Thread(() =>
            {
                Settings.Default.BranchId = 1;
                Settings.Default.CompanyId = 1;
                Settings.Default.Date = DateTime.Now.AddMinutes(-10);
                Settings.Default.Mode = "nutural";
                Settings.Default.Save();

                var log = new Log();
                Application.Run(log);
            });
            thread.Start();
            this.Close();

            //if (txtCode.Text.Length > 0 && txtPassword.Text.Length > 0)
            //{
            //    using (var client = new HttpClient())
            //    {
            //        var obj = new Dictionary<string, string>();
            //        obj.Add("username", txtCode.Text.Trim());
            //        obj.Add("password", txtPassword.Text.Trim());
            //        var data = JsonConvert.SerializeObject(obj);
            //        var url = ConfigurationManager.AppSettings["ApiUrl"].ToString() + "webappservices/login";
            //        var queryString = new StringContent(data, Encoding.UTF8, "text/plain");
            //        var result = await client.PostAsync(new Uri(url), queryString);
            //        string resultContent = await result.Content.ReadAsStringAsync();
            //        var res = JsonConvert.DeserializeObject<LoginResponse>(resultContent);
            //        if (res.id != null)
            //        {
            //            Settings.Default.BranchId = Convert.ToInt32(res.id);
            //            Settings.Default.CompanyId = Convert.ToInt32(res.id);
            //            Settings.Default.Date = DateTime.Now.AddMinutes(-10);
            //            Settings.Default.Mode = "nutural";
            //            Settings.Default.Save();

            //            var log = new Log();
            //            Application.Run(log);

            //            //var thread = new Thread(() =>
            //            //{
            //            //    var log = new Log();
            //            //    Application.Run(log);
            //            //});
            //            //thread.Start();
            //            this.Close();
            //        }
            //        else
            //        {
            //            MessageBox.Show("Login Invalid");
            //        }
            //    }
            //}
        }
    }

}
