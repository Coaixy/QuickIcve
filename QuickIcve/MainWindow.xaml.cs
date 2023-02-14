using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;

namespace QuickIcve
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string cookie = "";
        public MainWindow()
        {
            InitializeComponent();
            Login l = new Login();
            l.SendMsg = recevieInfo;
            l.Show();
        }

        public void recevieInfo(string data)
        {
            cookie ="auth="+data;
            initInfo();
        }
        private void initInfo()
        {
            var indexInfo = InfoHelper.indexInfo(cookie);
            var label = (Label)FindName("name");
            var tx = (Image)FindName("tx");
            if (label != null) label.Content = indexInfo["disPlayName"]?.ToString();
            RequestHelper.setTopImg(tx,indexInfo["avator"]?.ToString());
        }
    }
}