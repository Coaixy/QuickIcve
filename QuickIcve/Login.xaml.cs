using System.Linq;
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

        public sendMessage SendMessage;
        public Login()
        {
            InitializeComponent();
            // test();
        }

        private void inject()
        {
            var view = (WebBrowser)FindName("view");
            var a = view.Document as mshtml.HTMLDocument;
            var head = a.getElementsByTagName("head").Cast<HTMLHeadElement>().First();
            var script = (IHTMLScriptElement)a.createElement("script");
            head.appendChild((IHTMLDOMNode)script);
        }


        private void View_OnLoadCompleted(object sender, NavigationEventArgs e)
        {
            var view = (WebBrowser)FindName("view");
            if (view.Source.ToString() == "https://www.zjy2.icve.com.cn/student/studio/studio.html")
            {
                inject();
            }
        }
        
    }
}