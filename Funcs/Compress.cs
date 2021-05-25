using CommandLine;
using Ionic.Zip;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Utils.Libs;

namespace Utils.Funcs
{
    class Compress
    {
        [Verb("compress-seperate", HelpText = "分開壓縮在目錄中的檔案與目錄(只往下一層)。")]
        public class SeperateOptions : Common.BasicOptions
        {
            [Option('s', "source", Required = true, HelpText = "來源路徑，為絕對路徑。")]
            public string SourceDir { set; get; }
            [Option('d', "destination", Required = true, HelpText = "輸出路徑，為絕對路徑。")]
            public string DestinationDir { set; get; }
            [Option('o', "override", Required = false, HelpText = "是否覆蓋輸出資料夾。")]
            public bool OverrideDest { set; get; }
            [Option('p', "password", Required = false, HelpText = "壓縮密碼。")]
            public string Password { set; get; }
        }
        public Compress(SeperateOptions options)
        {
            Common.showTimestamp = options.showTimestamp;
            if (!CheckDirExists(options.SourceDir))
            {
                Common.WriteLog($"{options.SourceDir} 不存在");
                return;
            }
            // 存在且不覆蓋
            if (CheckDirExists(options.DestinationDir) && !options.OverrideDest)
            {
                Common.WriteLog($"{options.DestinationDir} 已存在");
                return;
            }
            if (options.coreNum == 0 || options.coreNum < -1)
            {
                Common.WriteLog($"並行工作數目上限設定錯誤");
                return;
            }
            CreateFolder(options.DestinationDir);
            DirectoryInfo di = new DirectoryInfo(options.SourceDir);
            ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = options.coreNum };
            // 每個資料夾一個壓縮檔
            Parallel.ForEach(di.GetDirectories(), parallelOptions, (item) =>
            {
                Zip(item.FullName, options.DestinationDir, password: options.Password);
                Common.WriteLog($"{item.FullName}\n");
            });
            // 每個檔案一個壓縮檔
            Parallel.ForEach(di.GetFiles(), parallelOptions, (item) =>
            {
                Zip(item.FullName, options.DestinationDir, password: options.Password, dir: false);
                Common.WriteLog($"{item.FullName}\n");
            });

        }
        static bool CheckDirExists(string dirName)
        {
            DirectoryInfo di = new DirectoryInfo(dirName);
            if (di.Exists)
                return true;
            return false;
        }
        public static void CreateFolder(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
            {
                directoryInfo.Delete(true);
            }
            directoryInfo.Create();
        }
        public static void Zip(string sourcePath, string destinationPath, string password = null, bool dir = true)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.Always;
                zip.AlternateEncoding = Encoding.UTF8;
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None;

                if (!string.IsNullOrEmpty(password))
                {
                    zip.Password = password;
                }

                if (dir)
                {
                    // 要建立目錄，裡面才是檔案
                    zip.AddDirectory(sourcePath, new DirectoryInfo(sourcePath).Name);
                    zip.Save($"{destinationPath}\\{new DirectoryInfo(sourcePath).Name}.zip");
                }
                else
                {
                    // 不要有目錄，因此為空字串
                    zip.AddFile(sourcePath, "");
                    zip.Save($"{destinationPath}\\{new FileInfo(sourcePath).Name}.zip");
                }
            }
        }
    }
}
