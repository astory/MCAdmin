using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace MCAdmin.Heartbeats
{
    static class PostRequest
    {
        private static bool setValidator = false;
        private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }

        public static string Send(string uri, string datastr)
        {
            try
            {
                if (!setValidator)
                {
                    setValidator = true;
                    ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateRemoteCertificate);
                }
            }
            catch { }

            try
            {
                byte[] data = System.Text.Encoding.ASCII.GetBytes(datastr);
                HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(uri);
                hwr.Proxy = null;
                hwr.Method = "POST";
                hwr.ContentType = "application/x-www-form-urlencoded";
                hwr.ContentLength = data.Length;
                Stream str = hwr.GetRequestStream();
                str.Write(data, 0, data.Length);
                str.Close();
                HttpWebResponse hwres = (HttpWebResponse)hwr.GetResponse();
                if (hwres.StatusCode != HttpStatusCode.OK) { hwres.Close(); return ""; }
                str = hwres.GetResponseStream();
                StreamReader sr = new StreamReader(str);
                string response = sr.ReadToEnd();
                sr.Close();
                str.Close();
                hwres.Close();
                return response;
            }
            catch { return ""; }
        }
    }
}
