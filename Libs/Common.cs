using CommandLine;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Utils.Libs
{
    class Common
    {
        public static bool showTimestamp;
        public static void WriteLog(string log)
        {
            if (showTimestamp)
                Console.Write("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), log);
            else
                Console.Write("{0}", log);
        }
        public static void OutFileJson(object obj, string 路徑加檔名)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            //string json = JsonConvert.SerializeObject(obj);
            using (StreamWriter sw = new StreamWriter(路徑加檔名, false, Encoding.UTF8))
            {
                sw.Write(json);
            }
        }
        public class BasicOptions
        {
            [Option("timestamp", Required = false, HelpText = "Log 是否顯示時戳。預設為否。")]
            public bool showTimestamp { set; get; }
            [Option("core-num", Required = false, HelpText = "並行工作數目上限。最小為 1，而 -1 代表無限制。預設為 -1。")]
            public int coreNum { set; get; } = -1;
        }
    }
}
