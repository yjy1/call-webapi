using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace 调用WebApi
{
    class Program
    {
        //参考资料：
        //作者：Ultron Group
        //博客地址：https://www.cnblogs.com/MuNet/p/6732338.html

        //C# 调用WebApi 
        #region 1.WebRequest方式
        #region Post：
        public static void WebRequestPost()
        {
            //string _postData = "{\"key1\": \"{\\\"key11\\\": \\\"value11\\\",\\\"key12\\\": \\\"value12\\\"}\",\"key2\": \"value2\"}";
            UserInfo userInfo = new UserInfo()
            {
                UserName = "little cat",
                Sex = "female",
                Phone = "131313",
                Description = "miao miao~",
                Hobby = "pingpong"
            };
            string _postData =  JsonConvert.SerializeObject(userInfo);
            var result = HttpPost("http://localhost:28411/api/OperationApi/AddUserInfo", _postData );
        }
        public static string HttpPost(string url, string body)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        public static HttpWebResponse HttpPost2(string url, string post, Encoding requestEncoding)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                if (request == null) 
                    return null;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            if (request == null) 
                return null;
            request.Method = "POST";
            request.ContentType = "application/json";

            //如果需要POST数据  
            if (!string.IsNullOrWhiteSpace(post))
            {
                StringBuilder buffer = new StringBuilder();
                buffer.AppendFormat("{0}", post);
                byte[] data = requestEncoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }
        #endregion

        #region Get：
        public static void WebRequestGet()
        {
            string result = HttpGet("http://localhost:28411/api/OperationApi/GetUserInfo?userName=ycjhhh");
        }
        public static string HttpGet(string url)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        #endregion

        #endregion

        #region 2.HttpClient 方式
        #region Post:
        private static dynamic HttpClientPostAsync()
        { 
            UserInfo userInfo = new UserInfo()
            {
                Id = 2,
                UserName = "little dog",
                Sex = "male",
                Phone = "131313",
                Description = "wang wang~~",
                Hobby = "pingpong"
            };
            string _postData = JsonConvert.SerializeObject(userInfo);
            using (var client = new HttpClient())
            {
                var result = client.PostAsync("http://localhost:28411/api/OperationApi/UpdateUserInfo", new StringContent(_postData, Encoding.UTF8, "application/json"));
                var resultStr = result.Result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject(resultStr);
            }
        }
        #endregion

        #region Get:
        public static dynamic HttpClientGet()
        {
            using (var client = new HttpClient())
            {
                var result = client.GetAsync("http://localhost:28411/api/OperationApi/GetUserInfo?userName=ycjhhh" ).Result;
                var resultStr = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject(resultStr);
            }
        }
        #endregion
        #endregion

        static void Main(string[] args)
        {
            WebRequestPost();
            WebRequestGet();

            HttpClientPostAsync();
            var result = HttpClientGet();
            Console.WriteLine();
        }
        /// <summary>
        /// 学生信息模型
        /// </summary>
        public class UserInfo
        {
            /// <summary>
            /// 学生编号
            /// </summary>
            public int? Id { get; set; }

            /// <summary>
            /// 学生姓名
            /// </summary>
            public string UserName { get; set; }

            /// <summary>
            /// 学生性别
            /// </summary>
            public string Sex { get; set; }

            /// <summary>
            /// 学生联系电话
            /// </summary>
            public string Phone { get; set; }

            /// <summary>
            /// 学生描述
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// 学生爱好
            /// </summary>
            public string Hobby { get; set; }
        }
    }
}
