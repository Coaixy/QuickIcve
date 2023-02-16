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
            cookie = data;
            initInfo();
        }
        private void initInfo()
        {
            var indexInfo = InfoHelper.indexInfo(cookie);
            if (name != null) name.Content = indexInfo["disPlayName"]?.ToString();
            RequestHelper.setTopImg(tx,indexInfo["avator"]?.ToString());
            
            var courseList = InfoHelper.courseList(cookie)["courseList"];
            List<CourseInfo> items = new List<CourseInfo>();
            foreach (var course in courseList)
            {
                var temp = new CourseInfo()
                {
                    courseOpenId = course["courseOpenId"]?.ToString(),
                    openClassId = course["openClassId"]?.ToString(),
                    name = course["courseName"]?.ToString(),
                    tName = course["assistTeacherName"]?.ToString(),
                    percent = Convert.ToInt32(course["process"])
                };
                items.Add(temp);
            }
            courseListView.ItemsSource = items;
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
                TabControl.SelectedIndex = 1;
                // MessageBox.Show(selectedItem.courseOpenId, selectedItem.openClassId);
                // MessageBox.Show(cookie);
                var studyList = InfoHelper.studyList(cookie, selectedItem.courseOpenId, selectedItem.openClassId)?["progress"]?["moduleList"];
                List<StudyInfo> items = new List<StudyInfo>();
                foreach (var studyItem in studyList)
                {
                    if (Convert.ToInt32(studyItem["percent"]) < 100)
                    {
                        var temp = new StudyInfo()
                        {
                            moduleId = studyItem["id"]?.ToString(),
                            name  = studyItem["name"]?.ToString(),
                            openClassId = selectedItem.openClassId,
                            percent = Convert.ToInt32(studyItem["percent"]),
                            status = "未开始"
                        };
                        items.Add(temp);
                    }
                }
                studyListView.ItemsSource = items;
            }
        }

        public class StudyInfo
        {
            public string moduleId { get; set; }
            public string openClassId { get; set; }
            public string name { get; set; }
            public int percent { get; set; }
            public string status { get; set; }
        }
        public class CourseInfo
        {
            public string courseOpenId { get; set; }
            public string openClassId { get; set; }
            public string name { get; set; }
            public string tName { get; set; }
            public int percent { get; set; }
        }

        private void start(object sender, RoutedEventArgs e)
        {
            
        }
    }
}