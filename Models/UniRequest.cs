using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;

namespace XiyouNews.Models
{
    public delegate void RequestFinishedHandler(UniResult ResponseResult);

    public class UniRequest
    {
        public static RequestFinishedHandler OnRequestFinished;

        public event RequestFinishedHandler RequestFinished
        {
            add
            {
                OnRequestFinished += new RequestFinishedHandler(value);
            }
            remove
            {
                OnRequestFinished -= new RequestFinishedHandler(value);
            }
        }

        string Url;

        WebClient wc = new WebClient();
        public UniRequest(string Url, bool IsAsync = false)
        {
            if (IsAsync == true)
            {
                wc.OpenReadCompleted += wc_OpenReadCompleted;
                wc.OpenReadAsync(new Uri(Url));
            }
            else
            {
                this.Url = Url;
            }
        }

        public UniResult DoRequest()
        {
            if(string.IsNullOrWhiteSpace(Url))
            {
                return new UniResult
                        {
                            Result = false,
                            Detail = null
                        };
            }
            Stream stream;
            try
            {
                stream = wc.OpenRead(new Uri(Url));
            }
            catch(WebException e)
            {
                if (((HttpWebResponse)e.Response).StatusCode.ToString() == "NotFound")
                    return new UniResult
                    {
                        Result = false,
                        Detail = "OUT_OF_RANGE"
                    }; 
                return new UniResult
                    {
                        Result = false,
                        Detail = null
                    };
            }
            StreamReader reader = new StreamReader(stream);
            string str = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            return new UniResult
                {
                    Result = true,
                    Detail = str
                };
                
        }

        void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                OnRequestFinished(new UniResult
                    {
                        Result = false,
                        Detail = null
                    });
                return;
            }

            using (Stream stream = e.Result)
            {
                StreamReader reader = new StreamReader(stream);
                string str = reader.ReadToEnd();
                OnRequestFinished(new UniResult
                    {
                        Result = true,
                        Detail = str
                    });
            }
        }
    }
}