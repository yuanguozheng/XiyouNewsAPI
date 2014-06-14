using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiyouNews.Models
{
    public class IndexInfo
    {
        static string[] Data = new string[7]
        {
            "xyyw","mtxy","bmxw","xydt","xyrw","qqxy","xwzt"
        };
        public static string GetUrl(int type, int page)
        {
            if (page == -1)
            {
                return "http://222.24.19.61/" + Data[type - 1] + ".htm";
            }
            else
            {
                return string.Format("http://222.24.19.61/{0}/{1}.htm", Data[type - 1], page);
            }
        }
    }
}