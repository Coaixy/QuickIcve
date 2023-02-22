using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuickIcve
{
    public class InfoHelper
    {
        public void Log(string text)
        {
            
        }
        public static JObject indexInfo(string cookie)
        {
            Dictionary<string, string> data = new Dictionary<string, string> { { "cookie", cookie } };
            var text = RequestHelper.Post("https://zjy2.icve.com.cn/api/student/Studio/index", data);
            var jsonData = JObject.Parse(text);
            return jsonData;
        }

        public static JObject courseList(string cookie)
        {
            Dictionary<string, string> data = new Dictionary<string, string> { { "cookie", cookie } };
            var text = RequestHelper.Post("https://zjy2.icve.com.cn/api/student/learning/getLearnningCourseList", data);
            var jsonData = JObject.Parse(text);
            return jsonData;
        }

        public static JObject studyList(string cookie, string courseOpenId, string openClassId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "cookie", cookie },
                { "courseOpenId", courseOpenId },
                { "openClassId", openClassId }
            };
            var text = RequestHelper.Post("https://zjy2.icve.com.cn/api/study/process/getProcessList", data);
            var jsonData = JObject.Parse(text);
            return jsonData;
        }
        public static JObject topicList(string cookie, string courseOpenId, string moduleId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "cookie", cookie },
                { "courseOpenId", courseOpenId },
                { "moduleId", moduleId }
            };
            var text = RequestHelper.Post("https://zjy2.icve.com.cn/api/study/process/getTopicByModuleId", data);
            var jsonData = JObject.Parse(text);
            return jsonData;
        }
        public static JObject cellList(string cookie, string courseOpenId, string openClassId,string topicId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "cookie", cookie },
                { "courseOpenId", courseOpenId },
                { "openClassId", openClassId },
                { "topicId", topicId }
            };
            var text = RequestHelper.Post("https://zjy2.icve.com.cn/api/study/process/getCellByTopicId", data);
            var jsonData = JObject.Parse(text);
            return jsonData;
        }
        public static JObject cellInfo(string cookie, string courseOpenId, string openClassId,string cellId,string moduleId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "cookie", cookie },
                { "courseOpenId", courseOpenId },
                { "openClassId", openClassId },
                { "cellId", cellId },
                { "moduleId", moduleId },
                { "flag", "s" }
            };
            var text = RequestHelper.Post("https://zjy2.icve.com.cn/api/common/Directory/viewDirectory", data);
            var jsonData = JObject.Parse(text);
            return jsonData;
        }
        public static string changeCourse(string cookie, string courseOpenId, string openClassId,string cellId,string moduleId,string cellName)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "cookie", cookie },
                { "courseOpenId", courseOpenId },
                { "openClassId", openClassId },
                { "cellId", cellId },
                { "cellName", cellName },
                { "moduleId", moduleId }
            };
            var text = RequestHelper.Post("https://zjy2.icve.com.cn/api/common/Directory/changeStuStudyProcessCellData", data);
            return text;
        }
        
        public static string sendData(string cookie, string courseOpenId, string openClassId,string cellId,string token
        ,string studyNewlyTime,string studyNewlyPicNum,string cellLogId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "cookie", cookie },
                { "courseOpenId", courseOpenId },
                { "openClassId", openClassId },
                { "cellId", cellId },
                { "cellLogId", cellLogId },
                { "studyNewlyTime", studyNewlyTime },
                { "studyNewlyPicNum", studyNewlyPicNum },
                { "token", token },
                { "picNum", studyNewlyPicNum },
                { "flag", "s" }
            };
            var text = RequestHelper.Post("https://zjy2.icve.com.cn/api/common/Directory/stuProcessCellLog", data);
            return text;
        }
    }
}