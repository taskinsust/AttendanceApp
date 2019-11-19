using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AttendanceApp.Models;
using log4net;
using Newtonsoft.Json;
 
namespace AttendanceApp.Helper
{
    public class HttpHelper
    {
        private static readonly ILog serviceLog = LogManager.GetLogger("ServiceLogger");

        #region Http Helper

        //Accept url and data to make a Http post request
        //return JsonResponse object
        public JsonResponse HttpPostRequest(string url, string data)
        {
            string output = String.Empty;
            var req = System.Net.WebRequest.Create(url);
            try
            {
                req.Method = "POST";
                req.Timeout = 1000000;
                req.ContentType = "application/x-www-form-urlencoded";
                byte[] sentData = Encoding.ASCII.GetBytes(data);
                req.ContentLength = sentData.Length;
                using (var sendStream = req.GetRequestStream())
                {
                    sendStream.Write(sentData, 0, sentData.Length);
                    sendStream.Close();
                }
                var response = req.GetResponse();
                output = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (WebException ex)
            {
                //Log.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                //Log.Error(ex);
                throw;
            }
            return ConvertToJsonResponse(output);
        }

        //Accept url to make a Http get request
        //return JsonResponse object
        public JsonResponse HttpGetRequest(string url)
        {
            string outputStr = String.Empty;
            var req = System.Net.WebRequest.Create(url);
            try
            {
                var resp = req.GetResponse();
                using (var stream = resp.GetResponseStream())
                {
                    if (stream != null)
                        using (var sr = new StreamReader(stream))
                        {
                            outputStr = sr.ReadToEnd();
                            sr.Close();
                        }
                }
            }
            catch (WebException ex)
            {
                var err = req + ex.Message;
               // Log.Error(err);
                throw;
            }
            catch (Exception ex)
            {
                //Log.Error(ex);
                throw;
            }
            return ConvertToJsonResponse(outputStr);
        }

        #endregion

        #region Private Methods
        private JsonResponse ConvertToJsonResponse(string response)
        {
            try
            {
                return JsonConvert.DeserializeObject<JsonResponse>(response);
            }
            catch (Exception ex)
            {
               // Log.Error(ex);
                throw;
            }
        }

        #endregion
    }
}
