using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace QuickIcve
{
    public class RequestHelper
    {
        //设置 头像
        public static void setTopImg(Image tx,string url)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource =
                new Uri(url,
                    UriKind.Absolute);
            bitmapImage.EndInit();
            tx.Source = bitmapImage;
        }

        public static string Post(string url, Dictionary<string, string> dic)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36 Edg/110.0.1587.41";
            StringBuilder sb = new StringBuilder();
            int i = 0;
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

            byte[] data = Encoding.UTF8.GetBytes(sb.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data,0,data.Length);
                reqStream.Close();
            }

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