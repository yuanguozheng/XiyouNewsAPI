using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XiyouNews.Models;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Web;

namespace XiyouNews.Controllers
{
    public class NewsListController : ApiController
    {
        static string[] TypeArray = { "西邮要闻", "媒体西邮", "部门新闻", "学院动态", "西邮人物", "菁菁校园", "新闻专题" };

        NewsListClass NewsList = new NewsListClass();

        bool IsOlderPages = false;

        [HttpGet]
        [HttpPost]
        public UniResult Index()
        {
            int type = -1, page = -1;
            try
            {
                type = int.Parse(HttpContext.Current.Request.Params["type"]);
            }
            catch
            {
                return new UniResult
                {
                    Result = false,
                    Detail = "TYPE_ERROR"
                };
            }
            try
            {
                page = int.Parse(HttpContext.Current.Request.Params["page"]);
            }
            catch
            {
                page = -1;
            }

            int p = page;

            if (p != -1)
                IsOlderPages = true;
            string URL;
            try
            {
                URL = IndexInfo.GetUrl((int)type, p);
            }
            catch
            {
                return new UniResult
                {
                    Result = false,
                    Detail = "TYPE_ERROR"
                };
            }
            if (type > 7 || type < 1)
            {
                return new UniResult
                {
                    Result = false,
                    Detail = "TYPE_ERROR"
                };
            }

            UniRequest req = new UniRequest(URL);
            UniResult reqResult = req.DoRequest();
            if (reqResult.Result != false)
            {
                ProcHTML(reqResult.Detail.ToString());
                NewsList.Type = new Models.Type
                {
                    ID = type,
                    Name = TypeArray[type - 1]
                };
                NewsList.CurrentURLPage = p;
                return new UniResult
                {
                    Result = true,
                    Detail = NewsList
                };
            }
            else
            {
                UniResult re = new UniResult
                {
                    Result = false,
                    Detail = reqResult.Detail
                };
                if (string.IsNullOrWhiteSpace(reqResult.Detail.ToString()))
                {
                    re.Detail = "SERVER_ERROR";
                }
                return re;
            }
        }

        private void ProcHTML(string content)
        {
            List<NewsListItemClass> Data = new List<NewsListItemClass>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);

            HtmlNode node = doc.GetElementbyId("fanye1097");
            string[] str = Regex.Split(node.InnerText, "&nbsp;");
            int amount = int.Parse(str[0].Replace("共", "").Replace("条", ""));
            string[] pagearr = Regex.Split(str[2], "/");
            int pages = int.Parse(pagearr[1]);
            int currentpage = int.Parse(pagearr[0]);
            NewsList.Pages = pages;  //总页数
            NewsList.CurrentPage = currentpage;  //当前页
            NewsList.Amount = amount;  //总条数

            node = doc.GetElementbyId("fylb");
            node = node.ChildNodes["UL"];
            foreach (var item in node.ChildNodes)
            {
                if (IsOlderPages && item.Id == "lineu5_0")
                    continue;
                if (item.Name == "li")
                {
                    string link = item.ChildNodes[0].ChildNodes[2].Attributes["href"].Value.Replace("../","");
                    string title = item.ChildNodes[0].ChildNodes[2].Attributes["title"].Value;
                    string date = item.ChildNodes[1].ChildNodes[0].InnerHtml.Replace("[", "").Replace("]&nbsp;", "");
                    Data.Add(new NewsListItemClass
                    {
                        Title = title,
                        Link = link,
                        Date = date
                    });
                }
                if (Data.Count == 20)
                    break;
            }
            NewsList.News = Data;
        }
    }
}
