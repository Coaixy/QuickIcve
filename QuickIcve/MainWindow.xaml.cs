using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
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
        ObservableCollection<CourseInfo> CourseViewItems = new ObservableCollection<CourseInfo>();
        ObservableCollection<StudyInfo> StudyViewItems = new ObservableCollection<StudyInfo>();
        ObservableCollection<cellInfo> CellViewItems = new ObservableCollection<cellInfo>();
        private string cookie = "";
        public MainWindow()
        {
            InitializeComponent();
            courseListView.ItemsSource = CourseViewItems;
            studyListView.ItemsSource = StudyViewItems;
            cellListView.ItemsSource = CellViewItems;
            
            Login l = new Login();
            l.SendMsg = recevieInfo;
            l.Show();
            
        }

        public void recevieInfo(string data)
        {
            cookie = data;
            initCourse();
        }
        private void initCourse()
        {
            var indexInfo = InfoHelper.indexInfo(cookie);
            var courseList = InfoHelper.courseList(cookie)["courseList"];
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
                CourseViewItems.Add(temp);
            }
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
                var studyList = InfoHelper.studyList(cookie, selectedItem.courseOpenId, selectedItem.openClassId)?["progress"]?["moduleList"];
                foreach (var studyItem in studyList)
                {
                    if (Convert.ToInt32(studyItem["percent"]) < 100)
                    {
                        var temp = new StudyInfo()
                        {
                            courseOpenId = selectedItem.courseOpenId,
                            moduleId = studyItem["id"]?.ToString(),
                            name  = studyItem["name"]?.ToString(),
                            openClassId = selectedItem.openClassId,
                            percent = Convert.ToInt32(studyItem["percent"]),
                            status = "未开始"
                        };
                        StudyViewItems.Add(temp);
                    }
                }
            }
        }
        
        public void start(object sender, RoutedEventArgs e)
        {
            var studyInfos = StudyViewItems;
            Thread study = new Thread(() =>
            {
                foreach (var studyInfo in studyInfos)
                {
                    var topicList = InfoHelper.topicList(cookie, studyInfo.courseOpenId, studyInfo.moduleId);
                    foreach (var topicInfo in topicList["topicList"])
                    {
                        var cellList = InfoHelper.cellList(cookie, studyInfo.courseOpenId, studyInfo.openClassId, topicInfo["id"].ToString());
                        foreach (var cellInfo in cellList["cellList"])
                        {
                            //存在子节点
                            if (cellInfo.ToString().Contains("childNodeList"))
                            {
                                foreach (var child in cellInfo["childNodeList"])
                                {
                                    {
                                        var info = InfoHelper.cellInfo(cookie, studyInfo.courseOpenId,
                                            studyInfo.openClassId, child["Id"].ToString(), studyInfo.moduleId);

                                        var temp = new cellInfo()
                                        {
                                            cellId = info["cellId"].ToString(),
                                            cellLogId = info["cellLogId"].ToString(),
                                            cellName = info["cellName"].ToString(),
                                            guIdToken = info["guIdToken"].ToString(),
                                            pageCount = info["pageCount"].ToString(),
                                            audioVideoLong = info["audioVideoLong"].ToString(),
                                            stuStudyNewlyPicCount = info["stuStudyNewlyPicCount"].ToString(),
                                            stuStudyNewlyTime = info["stuStudyNewlyTime"].ToString(),
                                            categoryName = info["categoryName"].ToString(),
                                            status = "No" //未开始
                                        };
                                        if (temp.stuStudyNewlyPicCount != temp.pageCount ||
                                            temp.stuStudyNewlyTime != temp.audioVideoLong)
                                        {
                                            Application.Current.Dispatcher.Invoke((Action)(() =>
                                            {
                                                CellViewItems.Add(temp);
                                            }));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                
                            }
                        }
                    }
                }
            });
            study.Start();
        }
        public class StudyInfo
        {
            public string courseOpenId { get; set; }
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
        public class cellInfo
        {
            public string cellId { get; set; }
            public string cellLogId { get; set; }
            public string audioVideoLong { get; set; }
            public string stuStudyNewlyTime { get; set; }
            public string stuStudyNewlyPicCount { get; set; }
            public string pageCount { get; set; }
            public string guIdToken { get; set; }
            public string categoryName { get; set; }
            public string cellName { get; set; }
            public string status { get; set; }
        }
    }
}