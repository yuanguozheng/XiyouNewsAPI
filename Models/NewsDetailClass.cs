using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiyouNews.Models
{
    public class NewsDetailClass
    {
        public string Title { get; set; }
        public string Editor { get; set; }
        public string Date { get; set; }
        public string From { get; set; }
        public string Passage { get; set; }
        public bool HasPic
        {
            get
            {
                if (Images.Count == 0)
                    return false;
                else
                    return true;
            }
        }
        public List<string> Images { get; set; }
        public int Browsed { get; set; }
        public string RawUrl { get; set; }
    }
}