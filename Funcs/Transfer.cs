using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Utils.Libs;
using Utils.Models;

namespace Utils.Funcs
{
    class Transfer
    {
        [Verb("transfer", HelpText = "ftp 傳輸。")]
        public class Options : Common.BasicOptions
        {
            [Option('h', "host", Required = true, HelpText = "Url，例如 ftp://127.0.0.1。")]
            public string Host { set; get; }
            [Option('d', "directory", Required = true, HelpText = "zip 檔案所在位置，為絕對路徑。")]
            public string Directory { set; get; }
            [Option("connection", Required = false, HelpText = "ftp 最大連線數。")]
            public int Connection { set; get; } = 2;
            [Option("finish-threshold", Required = false, HelpText = "連續幾秒內無連線數代表傳輸結束。")]
            public int FinishThreshold { set; get; } = 30;
        }
        public Transfer(Options options)
        {
            Common.showTimestamp = options.showTimestamp;


            DirectoryInfo di = new DirectoryInfo(options.Directory);
            List<FileInfo> fis = new List<FileInfo>(di.GetFiles());
            傳輸連線數 傳輸連線數 = new 傳輸連線數();
            資料Buffer<傳輸資訊> 傳輸資訊Buffer = new 資料Buffer<傳輸資訊>();
            bool finishFlag = false;
            int zeroTimes = 0;
            while (!finishFlag)
            {
                Parallel.ForEach(fis, new ParallelOptions { MaxDegreeOfParallelism = options.coreNum }, fi =>
                {
                    lock (傳輸連線數)
                    {
                        if (傳輸連線數.Connection >= options.Connection)
                            return;
                        if (!fi.Extension.Equals(".zip"))
                            return;
                        if (傳輸資訊Buffer.IsExist(fi.FullName))
                            return;
                        Thread t = new Thread(new ThreadStart(() =>
                        {
                            using (WebClient client = new WebClient())
                            {
                                傳輸資訊 傳輸資訊 = new 傳輸資訊()
                                {
                                    Filename = fi.FullName,
                                    StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                                };
                                lock (傳輸資訊Buffer)
                                {
                                    傳輸資訊Buffer.Add(傳輸資訊, fi.FullName);
                                }
                                Common.WriteLog($"{fi.FullName}\n");
                                client.Credentials = new NetworkCredential("test_cs", "1234");
                                client.UploadFile(Path.Combine(options.Host, fi.Name).Replace("\\", "/"), WebRequestMethods.Ftp.UploadFile, fi.FullName);
                                lock (傳輸連線數)
                                    傳輸連線數.Connection -= 1;
                                傳輸資訊.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            }
                        }));
                        t.Start();
                        傳輸連線數.Connection += 1;
                    }
                });
                Thread.Sleep(1000);
                if (傳輸連線數.Connection == 0)
                {
                    zeroTimes += 1;
                    Common.WriteLog($"\r已經 { zeroTimes } 秒內無連線數");
                }
                else
                {
                    zeroTimes = 0;
                }
                if (zeroTimes == options.FinishThreshold)
                {
                    finishFlag = true;
                }
            }
            Common.OutFileJson(傳輸資訊Buffer.ToList(), "傳輸資訊.json");
        }
    }
}
