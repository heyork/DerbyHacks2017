using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SQLite;
using DerbyHacks.Model;
using DerbyHacks.Biz;
using System.IO;
using System.Text;

namespace DerbyHacksApi.Controllers
{
    public class HomeFacesController : ApiController
    {
        private List<RecognitionCandidate> notifications;

        HomeFacesController()
        {
            notifications = new List<RecognitionCandidate>();
        }

        public IEnumerable<RecognitionCandidate> Get(HttpRequestMessage req)
        {
            if (notifications.Count > 0)
            {
                var ret = notifications;
                notifications = new List<RecognitionCandidate>();
                return ret;
            }

            return notifications;
        }

        public void Post([FromBody]string value)
        {
            //test gallery upload
            if (false)
            {
                WebRequest req = WebRequest.Create("https://api.kairos.com/enroll");
                req.Method = "POST";
                req.ContentType = "application/json";
                req.Headers.Add("app_id", "cd10d73e");
                req.Headers.Add("app_key", "df0b63f77f0ed238934be08aeb5ff42d");

                //string p = "{\"image\"=\"" + Convert.ToBase64String(File.ReadAllBytes("C:\\homeFaces.png")) + "\", \"subject_id\":\"ryan\", \"gallery_name\":\"DerbyHacks\"}";
                string p = "{\"image\":\"http://i.imgur.com/4p2RQvu.jpg\",\"subject_id\":\"Elizabeth\",\"gallery_name\":\"MyGallery\"}";

                req.ContentLength = p.Length;
                StreamWriter s = new StreamWriter(req.GetRequestStream());
                s.Write(p);
                s.Close();

                try
                {
                    var httpResponse = (HttpWebResponse)req.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var r = streamReader.ReadToEnd();
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            else
            {

                //Recognize face from client app
                WebRequest request = WebRequest.Create("https://api.kairos.com/recognize");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("app_id", "cd10d73e");
                request.Headers.Add("app_key", "df0b63f77f0ed238934be08aeb5ff42d");

                //string postData = string.Format(@"gallery_name=MyGallery&image={0}", Convert.ToBase64String(File.ReadAllBytes("C:\\homeFaces.png")));
                string postData = "{\"image\":\"" + Convert.ToBase64String(File.ReadAllBytes("C:\\homeFaces.png")) + "\", \"gallery_name\":\"MyGallery\"}";
                byte[] bytes = Encoding.UTF8.GetBytes(postData);

                request.ContentLength = bytes.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();

                WebResponse response = request.GetResponse();
                var test = ((HttpWebResponse)response).StatusDescription;
                stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string result = reader.ReadToEnd();
            }
        }
    }
}
