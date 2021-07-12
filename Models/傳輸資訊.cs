using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Models
{
    class 傳輸資訊
    {
        public 傳輸資訊()
        {
            Filename = "";
            StartTime = "";
            EndTime = "";
        }
        public string Filename { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
