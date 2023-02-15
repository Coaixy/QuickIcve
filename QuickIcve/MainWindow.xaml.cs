using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            if (name != null) name.Content = indexInfo["disPlayName"]?.ToString();
            RequestHelper.setTopImg(tx,indexInfo["avator"]?.ToString());
            
            var courseList = InfoHelper.curseList(cookie)["courseList"];
            List<CourseInfo> items = new List<CourseInfo>();
            foreach (var course in courseList)
            {
                var temp = new CourseInfo()
                {
                    id = course["Id"].ToString(),
                    name = course["courseName"].ToString(),
                    tName = course["assistTeacherName"].ToString(),
                    percent = Convert.ToInt32(course["process"].ToString())
                };
                items.Add(temp);
            }
            courseListView.ItemsSource = items;
        }
        
        public class CourseInfo
        {
            public string id { get; set; }
            public string name { get; set; }
            public string tName { get; set; }
            public int percent { get; set; }
        }

        private void listDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = (CourseInfo)courseListView.SelectedItem;
            if (selectedItem.percent == 100)
            {
                MessageBox.Show("此课程已达到100%", "提示");
            }
            else
            {
                
            }
        }
    }
}