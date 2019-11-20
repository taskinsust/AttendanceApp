using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceApp.Models
{
    public class JsonResponse
    {
        public bool IsSuccess { get; set; }
        public dynamic Data { get; set; }

        public JsonResponse()
        {
        }

        public JsonResponse(bool isSuccess, dynamic data)
        {
            this.IsSuccess = isSuccess;
            this.Data = data;
        }
    }
}
