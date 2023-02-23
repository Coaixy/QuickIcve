using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
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
        private CourseInfo SelectedCourse = new CourseInfo(); 
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
            // var indexInfo = InfoHelper.indexInfo(cookie);
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
            
            SelectedCourse = (CourseInfo)courseListView.SelectedItem;
            if (SelectedCourse.percent == 100)
            {
                MessageBox.Show("此课程已达到100%", "提示");
            }
            else
            {
                TabControl.SelectedIndex = 1;
                var studyList = InfoHelper.studyList(cookie, SelectedCourse.courseOpenId, SelectedCourse.openClassId)?["progress"]?["moduleList"];
                foreach (var studyItem in studyList)
                {
                    if (Convert.ToInt32(studyItem["percent"]) < 100)
                    {
                        var temp = new StudyInfo()
                        {
                            courseOpenId = SelectedCourse.courseOpenId,
                            moduleId = studyItem["id"]?.ToString(),
                            name  = studyItem["name"]?.ToString(),
                            openClassId = SelectedCourse.openClassId,
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
            TabControl.SelectedIndex = 2;
            var studyInfos = StudyViewItems;
            var nowCellViewItems =  new ObservableCollection<cellInfo>();
            cellListView.ItemsSource = nowCellViewItems;
            Task study = new Task(() =>
            {
                //获取所有Cell的信息
                foreach (var studyInfo in studyInfos)
                {
                    var topicList = InfoHelper.topicList(cookie, studyInfo.courseOpenId, studyInfo.moduleId);
                    foreach (var topicInfo in topicList["topicList"])
                    {
                        var cellList = InfoHelper.cellList(cookie, studyInfo.courseOpenId, studyInfo.openClassId, topicInfo["id"].ToString());
                        foreach (var cellInfo in cellList["cellList"])
                        {
                            //存在子节点
                            if (cellInfo["childNodeList"].Any())
                            {
                                foreach (var child in cellInfo["childNodeList"])
                                {
                                    AddCellItems(studyInfo.courseOpenId,
                                        studyInfo.openClassId, cellInfo["Id"]?.ToString(), studyInfo.moduleId);
                                }
                            }//不存在子节点
                            else
                            {
                                AddCellItems(studyInfo.courseOpenId,
                                    studyInfo.openClassId, cellInfo["Id"]?.ToString(), studyInfo.moduleId);
                            }
                        }
                    }
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    nowCell.Content = "获取完毕";
                    
                });
                foreach (var cellInfo in CellViewItems)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        nowCellViewItems.Add(cellInfo);
                    });
                }
                //刷课
                foreach (var cellInfo in CellViewItems)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        nowCell.Content = cellInfo.cellName;
                    });
                    float audioVideoLonglength = Convert.ToSingle(cellInfo.audioVideoLong);
                    var newlyTime = Convert.ToSingle(cellInfo.stuStudyNewlyTime);
                    var newlyPicCount = Convert.ToInt32(cellInfo.stuStudyNewlyPicCount);
                    var pageCount = Convert.ToInt32(cellInfo.pageCount);

                    var debug_info = "";
                    //首包
                    var temp = JObject.Parse(InfoHelper.sendData(cookie,SelectedCourse.courseOpenId,SelectedCourse.openClassId,
                        cellInfo.cellId,cellInfo.guIdToken,newlyTime.ToString(),newlyPicCount.ToString(),cellInfo.cellLogId));
                    if (temp["code"].ToString() == "-100")
                    {
                        
                        InfoHelper.changeCourse(cookie,temp["currCourseOpenId"]?.ToString(),temp["currOpenClassId"]?.ToString(),
                            temp["curCellId"]?.ToString(),temp["currModuleId"]?.ToString(),temp["currCellName"]?.ToString());
                        Thread.Sleep(300);
                        var t = InfoHelper.sendData(cookie, SelectedCourse.courseOpenId, SelectedCourse.openClassId,
                            cellInfo.cellId, cellInfo.guIdToken, newlyTime.ToString(), newlyPicCount.ToString(),
                            cellInfo.cellLogId);
                    }
                    if (pageCount == 0) //音视频类
                    {
                        while (newlyTime != audioVideoLonglength) //没有跑完
                        {
                            Thread.Sleep(1000*10);
                            if (audioVideoLonglength - newlyTime <= 10) //防止溢出
                            {
                                newlyTime = audioVideoLonglength;
                            }
                            else //正常加减
                            {
                                newlyTime += 10;
                            }
                            var d = InfoHelper.sendData(cookie,SelectedCourse.courseOpenId,SelectedCourse.openClassId,
                                cellInfo.cellId,cellInfo.guIdToken,newlyTime.ToString(),newlyPicCount.ToString(),cellInfo.cellLogId);
                            if (!d.Contains("操作成功"))
                            {
                                newlyPicCount -= 1;
                            }
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                float value = (float)newlyTime / audioVideoLonglength * 100;
                                CellProgressBar.Value = Convert.ToInt32(value);
                            });
                        }
                    }
                    //文档类
                    else
                    {
                        while (newlyPicCount != pageCount)
                        {
                            Thread.Sleep(1000*10);
                            newlyPicCount += 1;
                            var d = InfoHelper.sendData(cookie,SelectedCourse.courseOpenId,SelectedCourse.openClassId,
                                cellInfo.cellId,cellInfo.guIdToken,newlyTime.ToString(),newlyPicCount.ToString(),cellInfo.cellLogId);
                            if (!d.Contains("操作成功"))
                            {
                                newlyPicCount -= 1;
                            }
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                float value = (float)newlyPicCount / pageCount * 100;
                                CellProgressBar.Value = Convert.ToInt32(value);
                            });
                        }
                    }
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        nowCellViewItems.Remove(cellInfo);
                        CellProgressBar.Value = 0;
                        cellListView.SelectedIndex = 0;
                    });
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TabControl.SelectedIndex = 0;
                    MessageBox.Show("完成任务");
                });
            });
            study.Start();
        }

        private void RefreshProgress()
        {
            var courseInfo = (CourseInfo)courseListView.SelectedItem;
            courseListView.ItemsSource = new List<CourseInfo>();
            studyListView.ItemsSource = new List<StudyInfo>();
            
            CourseViewItems.Clear();
            StudyViewItems.Clear();
            
            initCourse();
            
            var studyList = InfoHelper.studyList(cookie, courseInfo.courseOpenId, courseInfo.openClassId)?["progress"]?["moduleList"];
            foreach (var studyItem in studyList)
            {
                if (Convert.ToInt32(studyItem["percent"]) < 100)
                {
                    var temp = new StudyInfo()
                    {
                        courseOpenId = SelectedCourse.courseOpenId,
                        moduleId = studyItem["id"]?.ToString(),
                        name  = studyItem["name"]?.ToString(),
                        openClassId = SelectedCourse.openClassId,
                        percent = Convert.ToInt32(studyItem["percent"]),
                        status = "未开始"
                    };
                    StudyViewItems.Add(temp);
                }
            }
            
        }
        private void AddCellItems(string courseOpenId,string openClassId,string cellId,string moduleId)
        {
            {

                var info = InfoHelper.cellInfo(cookie, courseOpenId,
                    openClassId, cellId, moduleId);
                if (info["code"].ToString() is "-100")
                { 
                    InfoHelper.changeCourse(cookie,info["currCourseOpenId"]?.ToString(),info["currOpenClassId"]?.ToString(),
                        info["curCellId"]?.ToString(),info["currModuleId"]?.ToString(),info["currCellName"]?.ToString());
                    Thread.Sleep(300);
                    info = InfoHelper.cellInfo(cookie, courseOpenId,
                        openClassId, cellId, moduleId);
                }
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
                };
                if (temp.stuStudyNewlyPicCount != temp.pageCount ||
                    temp.stuStudyNewlyTime != temp.audioVideoLong)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        CellViewItems.Add(temp);
                    });
                }
            }
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
            
        }
        
    }
}