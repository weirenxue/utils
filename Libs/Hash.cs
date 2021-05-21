using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Utils.Libs
{
    class Hash
    {
        [Verb("hash", HelpText = "計算雜湊值。")]
        public class Options : Common.BasicOptions
        {
            [Option('d', "directory", Required = false, HelpText = "source 是否為目錄路徑，若為目錄路徑則計算目錄下一層所有檔案的雜湊值，若否則視為檔案路徑。預設為檔案路徑。")]
            public bool IsDirectory { set; get; }
            [Option('p', "path", Required = true, HelpText = "路徑，為絕對路徑。")]
            public IEnumerable<string> Path { set; get; }
            [Option('h', "hash", Required = true, HelpText = "雜湊演算法(md5, sha256)。")]
            public string Hash { set; get; }
            [Option('s', "show-full-path", Required = false, HelpText = "輸出雜湊值時，是否印出完整路徑，若否只印出檔案名稱。預設為否。")]
            public bool ShowFullPath { set; get; }
        }
        public Hash(Options options)
        {
            Common.showTimestamp = options.showTimestamp;
            switch (options.Hash)
            {
                case "md5":
                case "sha256":
                    break;
                default:
                    Common.WriteLog($"不支援 {options.Hash}");
                    return;
            }
            ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = options.coreNum };
            List<FileInfo> fis = new List<FileInfo>();
            if (options.IsDirectory)
            {
                // 將目錄下一層檔案都抓進 List
                options.Path.ToList().ForEach((item) =>
                {
                    DirectoryInfo di = new DirectoryInfo(item);
                    if (!di.Exists)
                    {
                        Common.WriteLog($"{item} not exist!\n");
                        return;
                    }
                    fis = fis.Concat(di.GetFiles()).ToList();
                });
            }
            else
            {
                // 將所有檔案都抓進 List
                options.Path.ToList().ForEach((item) =>
                {
                    FileInfo fi = new FileInfo(item);
                    if (!fi.Exists)
                    {
                        Common.WriteLog($"{item} not exist!\n");
                        return;
                    }
                    fis.Add(fi);
                });

            }
            // 計算所有檔案 MD5
            Parallel.ForEach(fis, parallelOptions, (item) =>
            {
                string name = options.ShowFullPath ? item.FullName : item.Name;
                string hash = "";
                switch (options.Hash)
                {
                    case "md5":
                        hash = FileMD5(item.FullName);
                        break;
                    case "sha256":
                        hash = FileSHA256(item.FullName);
                        break;
                }
                Common.WriteLog($"{name} {hash}\n");
            });
        }
        public static string FileMD5(string path)
        {
            if (!File.Exists(path))
            {
                return "";
            }
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(path))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        public static string FileSHA256(string path)
        {
            string SHACode = "";
            if (File.Exists(path))
            {
                byte[] hashValue;
                using (SHA256 mySHA256 = SHA256.Create())
                {
                    using (FileStream fs = File.OpenRead(path))
                    {
                        fs.Position = 0;
                        hashValue = mySHA256.ComputeHash(fs);
                    }
                }
                for (int i = 0; i < hashValue.Length; i++)
                {
                    SHACode += $"{hashValue[i]:x2}";
                }
            }
            return SHACode;
        }
    }
}
