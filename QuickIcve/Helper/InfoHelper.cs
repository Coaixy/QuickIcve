using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuickIcve
{
    public class InfoHelper
    {
        public static JObject indexInfo(string cookie)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("cookie",cookie);
            var text = RequestHelper.Post("https://zjy2.icve.com.cn/api/student/Studio/index", data);
            var jsonData = JObject.Parse(text);
            return jsonData;
        }

        public static JObject courseList(string cookie)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("cookie",cookie);
            var text = RequestHelper.Post("https://zjy2.icve.com.cn/api/student/learning/getLearnningCourseList", data);
            var jsonData = JObject.Parse(text);
            return jsonData;
        }

        public static JObject studyList(string cookie, string courseOpenId, string openClassId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("cookie",cookie);
            data.Add("courseOpenId",courseOpenId);
            data.Add("openClassId",openClassId);
            var text = RequestHelper.Post("https://zjy2.icve.com.cn/api/study/process/getProcessList", data);
            var jsonData = JObject.Parse(text);
            return jsonData;
        }
        public static JObject topicList(string cookie, string courseOpenId, string moduleId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("cookie",cookie);
            data.Add("courseOpenId",courseOpenId);
            data.Add("moduleId",moduleId);
            var text = RequestHelper.Post("https://zjy2.icve.com.cn/api/study/process/getTopicByModuleId", data);
            var jsonData = JObject.Parse(text);
            return jsonData;
        }
        public static JObject cellList(string cookie, string courseOpenId, string openClassId,string topicId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("cookie",cookie);
            data.Add("courseOpenId",courseOpenId);
            data.Add("openClassId",openClassId);
            data.Add("topicId",topicId);
            var text = RequestHelper.Post("https://zjy2.icve.com.cn/api/study/process/getCellByTopicId", data);
            var jsonData = JObject.Parse(text);
            return jsonData;
        }
        public static JObject cellInfo(string cookie, string courseOpenId, string openClassId,string cellId,string moduleId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("cookie",cookie);
            data.Add("courseOpenId",courseOpenId);
            data.Add("openClassId",openClassId);
            data.Add("cellId",cellId);
            data.Add("moduleId",moduleId);
            data.Add("flag","s");
            var text = RequestHelper.Post("https://zjy2.icve.com.cn/api/common/Directory/viewDirectory", data);
            var jsonData = JObject.Parse(text);
            return jsonData;
        }
    }
}