using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XiyouNews.Models
{
    public class NewsListClass
    {
        public Type Type { get; set; }
        public int Amount { get; set; }
        public int CurrentAmount
        {
            get
            {
                return News.Count;
            }
        }
        public int CurrentPage { get; set; }
        public int CurrentURLPage { get; set; }
        public int Pages { get; set; }
        public List<NewsListItemClass> News { get; set; }
    }
    public class Type
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}