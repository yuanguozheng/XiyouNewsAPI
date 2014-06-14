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
    public class NewsDetailController : ApiController
    {
        NewsDetailClass NewsDetail = new NewsDetailClass();
        [AcceptVerbs("GET", "POST")]
        public UniResult Index()
        {
            UniResult result = new UniResult();
            string link="";
            bool pic = false;
            try
            {
                link = HttpContext.Current.Request.Params["link"];
            }
            catch
            {
                result.Result = false;
                result.Detail = "PARAM_ERROR";
                return result;
            }
            try
            {
                pic = bool.Parse(HttpContext.Current.Request.Params["pic"]);
            }
            catch
            {
                pic = false;
            }
            if (string.IsNullOrWhiteSpace(link))
            {
                result.Result = false;
                result.Detail = "PARAM_ERROR";
                return result;
            }
            if (pic == null)
            {
                pic = false;
            }
            string url = string.Format("http://222.24.19.61/{0}", link);

            UniRequest req = new UniRequest(url);
            UniResult res = req.DoRequest();
            if (res.Result == false)
            {
                result.Detail = res.Detail;
                if (res.Detail == null)
                    result.Detail = "SERVER_ERROR";
                return result;
            }
            try
            {
                ProcHTML(res.Detail.ToString(), (bool)pic, url);
            }
            catch
            {
                result.Result = false;
                result.Detail = "PROC_ERROR";
                return result;
            }
            result.Result = true;
            result.Detail = NewsDetail;
            return result;
        }

        private void ProcHTML(string content, bool pic, string URL)
        {
            List<string> imgs = new List<string>();
            string title = null, date = null, from = null, editor = null, passage = null;
            int Browsed = 0;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            HtmlNode node = doc.GetElementbyId("nrys");

            int i = 0;
            foreach (var item in node.ChildNodes)
            {
                if (i == 2)
                    break;
                if (item.Name == "div")
                {
                    if (i == 0)
                        title = item.InnerText;
                    if (i == 1)
                    {
                        string[] t = Regex.Split(item.InnerText, "&nbsp;");
                        for (int n = 0; n < t.Length - 1; n++)
                        {
                            string[] p = Regex.Split(t[n], "：");
                            switch (n)
                            {
                                case 0: date = p[1]; break;
                                case 1: from = p[1]; break;
                                case 2: editor = p[1]; break;
                            }
                        }
                    }
                    i++;
                }
            }

            node = doc.GetElementbyId("vsb_newscontent");

            if (pic)
            {
                HtmlDocument ddoc = new HtmlDocument();
                ddoc.LoadHtml(node.InnerHtml);
                HtmlNodeCollection nc = ddoc.DocumentNode.SelectNodes("//img");
                if (nc != null)
                {
                    foreach (var nitem in nc)
                    {
                        string RawUrl = nitem.Attributes["src"].Value;
                        RawUrl = RawUrl.Substring(RawUrl.IndexOf("_mediafile"));
                        RawUrl = "http://222.24.19.61/" + RawUrl;
                        imgs.Add(RawUrl);
                    }
                }
            }

            HtmlDocument ndoc = new HtmlDocument();
            ndoc.LoadHtml(node.InnerHtml);
            HtmlNodeCollection imgnodes = ndoc.DocumentNode.SelectNodes("//table[@class='imgtable']");
            if (imgnodes == null)
                imgnodes = ndoc.DocumentNode.SelectNodes("//table[@class='winstyle597122029_16']");
            if (imgnodes == null)
                imgnodes = ndoc.DocumentNode.SelectNodes("//div[@style='text-align: center']");
            if (imgnodes != null)
            {
                foreach (var iitem in imgnodes)
                {
                    node.InnerHtml = node.InnerHtml.Replace(iitem.InnerHtml, "");
                }
            }
            string a = node.InnerText.Trim();
            int m = a.Length - 1;
            while (a[m] == '\n' || a[m] == '\r' || a[m] == ' ' || char.IsWhiteSpace(a[m]) || (int)a[m] == 8203)
            {
                m--;
            }
            a = a.Substring(0, m + 1);
            passage = a;

            string[] tmp = Regex.Split(URL, "/");
            string passageid = tmp[tmp.Length - 1].Replace(".htm", "");

            UniRequest req = new UniRequest(string.Format("http://222.24.19.61/system/resource/code/news/click/dynclicks.jsp?clickid={0}&clicktype=wbnews&owner=1046261409 ", passageid));
            UniResult res = req.DoRequest();
            if (res.Result != false)
            {
                Browsed = int.Parse(res.Detail.ToString());
            }

            NewsDetail = new NewsDetailClass
            {
                Title = title,
                Date = date,
                Editor = editor,
                From = from,
                Passage = passage,
                Images = imgs,
                Browsed = Browsed,
                RawUrl = URL
            };
        }
    }
}
