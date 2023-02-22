using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace QuickIcve
{
    public class RequestHelper
    {
        public static string Post(string url, Dictionary<string, string> dic,string test_data = "",bool text = false)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            req.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36 Edg/110.0.1587.41";
            StringBuilder sb = new StringBuilder();
            int i = 0;
            if (text == false)
            {
                foreach (var item in dic)
                {
                    if (item.Key == "cookie")
                    {
                        req.Headers.Add("Cookie",item.Value);
                    }
                    else
                    {
                        if (i!=0)
                        {
                            sb.Append("&");
                        }
                        sb.AppendFormat("{0}={1}", item.Key, item.Value);
                        i++;
                    }
                }
            }
            else
            {
                sb.Append(test_data);
            }
            byte[] data = Encoding.UTF8.GetBytes(sb.ToString());
            req.ContentLength = data.Length;
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(data,0,data.Length);
            reqStream.Flush();
            reqStream.Close();

            //
            // if (url.Contains("CellLog"))
            // {
            //     MessageBox.Show(sb.ToString());
            // }
            //
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream,Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

    }
}