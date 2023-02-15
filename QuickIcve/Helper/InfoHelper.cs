using System.Collections.Generic;
using System.Windows;
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

        public static JObject curseList(string cookie)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("cookie",cookie);
            var text = RequestHelper.Post("https://zjy2.icve.com.cn/api/student/learning/getLearnningCourseList", data);
            var jsonData = JObject.Parse(text);
            return jsonData;
        }
    }
}