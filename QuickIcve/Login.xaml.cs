using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using mshtml;

namespace QuickIcve
{
    public partial class Login : Window
    {
        public delegate void sendMessage(string data);

        public sendMessage SendMsg;
        public Login()
        {
            InitializeComponent();

        }
        private void View_OnLoadCompleted(object sender, NavigationEventArgs e)
        {
            var view = (WebBrowser)FindName("view");
            if (view.Source.ToString() == "https://www.zjy2.icve.com.cn/student/studio/studio.html")
            {
                var cookie = GetCookie("https://www.zjy2.icve.com.cn/student/studio/studio.html");
                SendMsg(cookie);
                Close();
            }
        }
        
        //用于获取ReadOnly的cookie
        private const int INTERNET_COOKIE_HTTPONLY = 0x00002000;
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetGetCookieEx(
            string url,
            string cookieName,
            StringBuilder cookieData,
            ref int size,
            int flags,
            IntPtr pReserved);
        public static string GetCookie(string url)
        {
            int size = 512;
            StringBuilder sb = new StringBuilder(size);
            if (!InternetGetCookieEx(url, null, sb, ref size, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero))
            {
                if (size < 0)
                {
                    return null;
                }
                sb = new StringBuilder(size);
                if (!InternetGetCookieEx(url, null, sb, ref size, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero))
                {
                    return null;
                }
            }
            return sb.ToString();
        }
    }
}