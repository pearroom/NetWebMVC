/*苏兴迎 E-Mail:284238436@qq.com*/
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MVC.Command
{
    public class Controller
    {
        public HttpListenerRequest Request = null;
        public HttpListenerResponse Response = null;
        public HttpSession Session = null;
        public Hashtable Params = new Hashtable();
        private string content = "";
        private string view = "";
        private FileItem fileitem = null;
        private string url_suffix = "";
        private Compress compress = null;
        public static Hashtable pageCacheList = new Hashtable();
        public Controller()
        {
            fileitem = new FileItem();
        }
        public void Init(HttpListenerContext context, HttpSession Session_, string view_, string url_suffix_, Compress compress_)
        {
            url_suffix = url_suffix_;
            compress = compress_;
            view = view_;
            Session = Session_;
            Request = context.Request;
            Response = context.Response;

        }
        /// <summary>
        /// 判断是否POST方法
        /// </summary>
        /// <returns></returns>
        public bool isPOST()
        {
            return Request.HttpMethod.ToUpper().Equals("POST");
        }
        /// <summary>
        /// 判断是否GET方法
        /// </summary>
        /// <returns></returns>
        public bool isGET()
        {
            return Request.HttpMethod.ToUpper().Equals("GET");
        }
        /// <summary>
        /// 判断是否Put方法
        /// </summary>
        /// <returns></returns>
        public bool isPut()
        {
            return Request.HttpMethod.ToUpper().Equals("PUT");
        }
        /// <summary>
        /// 判断是否Any方法
        /// </summary>
        /// <returns></returns>
        public bool isAny()
        {
            return Request.HttpMethod.ToUpper().Equals("ANY");
        }
        /// <summary>
        /// 判断是否Delete方法
        /// </summary>
        /// <returns></returns>
        public bool isDelete()
        {
            return Request.HttpMethod.ToUpper().Equals("DELETE");
        }
        /// <summary>
        /// 判断是否Head方法
        /// </summary>
        /// <returns></returns>
        public bool isHead()
        {
            return Request.HttpMethod.ToUpper().Equals("HEAD");
        }
        /// <summary>
        /// 判断是否Patch方法
        /// </summary>
        /// <returns></returns>
        public bool isPatch()
        {
            return Request.HttpMethod.ToUpper().Equals("PATCH");
        }
        /// <summary>
        /// 获取RESTFull风格参数
        /// </summary>
        /// <param name="index">参数下标</param>
        /// <returns></returns>
        public string InputById(int index)
        {
            string[] ss = url_suffix.Split('/');
            if (ss.Length > 1 && index < ss.Length && index > 0)
                return ss[index];
            else

                return "";
        }
        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns></returns>
        public string Input(string key)
        {
            if (Request.HttpMethod.Equals("GET"))
                return Get(key);
            else
            {
                return Form(key);
            }
        }
        /// <summary>
        /// 请求内容
        /// </summary>
        /// <returns></returns>
        public string Content()
        {
            if (content.Equals(""))
            {
                int len = (int)Request.ContentLength64;
                byte[] sdata = new byte[len];
                Request.InputStream.Read(sdata, 0, len);
                content = Encoding.UTF8.GetString(sdata);

               
            }

            return content;
        }
        /// <summary>
        /// 获取上传文件
        /// </summary>
        /// <returns></returns>
        public FileItem Files()
        {

            string type = Request.ContentType.ToString();
            string s = Request.TransportContext.ToString();
            int k = type.IndexOf("multipart/form-data");
            if (fileitem.FileName == null && k >= 0)
            {
                int len = (int)Request.ContentLength64;
                byte[] sdata = new byte[len];
                byte[] tmp = new byte[1024];
                int kk1 = 0, kk2 = 0;

                while (true)
                {
                    kk1 = Request.InputStream.Read(tmp, 0, tmp.Length);
                    if (kk1 > 0)
                    {
                        Array.Copy(tmp, 0, sdata, kk2, kk1);
                        kk2 = kk2 + kk1;
                    }
                    else
                    {
                        break;
                    }
                }

                if (len > 512)
                {
                    tmp = new byte[512];
                }
                else
                {
                    tmp = new byte[len];
                }
                Array.Copy(sdata, tmp, tmp.Length);
                string content = Encoding.UTF8.GetString(tmp);
                if (fileitem.FileName == null)
                {
                    string[] ss = content.Split('\n');
                    int leng = 0;
                    int hleng = Encoding.UTF8.GetBytes(ss[0].Trim() + "--").Length + 4;

                    for (int i = 0; i < ss.Length; i++)
                    {
                        string s1 = ss[i].ToString().Replace('\r', ' ').Trim();
                        if (s1.IndexOf("form-data") >= 0)
                        {
                            string[] tmps = s1.Split(';');
                            fileitem.FileName = tmps[2].Split('=')[1].Replace('\"', ' ').Trim();
                        }
                        leng += Encoding.UTF8.GetBytes(s1).Length + 2;
                        if (s1.Trim().Equals(""))
                        {
                            fileitem.FileStream = new byte[sdata.Length - leng - hleng];
                            Array.Copy(sdata, leng, fileitem.FileStream, 0, fileitem.FileStream.Length);

                            break;
                        }

                    }
                }

            }

            return fileitem;
        }
        /// <summary>
        /// 获取GET方法提交参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            string value = Request.QueryString.Get(key);
            if (!string.IsNullOrEmpty(value))
                value = Encoding.GetEncoding(Config.document_charset).GetString(Request.ContentEncoding.GetBytes(value));
            return value;

        }
        /// <summary>
        /// 接收Post方法提交参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Form(string key)
        {
            string value = "";

            string type = Request.ContentType.ToString();
            int k = type.IndexOf("application/x-www-form-urlencoded");
            if (k >= 0)
            {
                content = Content();
                string[] ps = content.Split('&');
                key = key + "=";
                foreach (string item in ps)
                {
                   
                    if (item.Length >= key.Length && item.Substring(0, key.Length).Equals(key))
                    {
                        value = item.Substring(item.IndexOf("=") + 1);
                        if (!string.IsNullOrEmpty(value))
                            value = Uri.UnescapeDataString(value);
                        break;
                    }
                }
            }

            return value;
        }
        /// <summary>
        /// HTML中传递数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetAttr(string key, object value)
        {
            if (Params.ContainsKey(key))
                Params[key] = value;
            else
                Params.Add(key, value);
        }
        /// <summary>
        /// 输出html页面
        /// </summary>
        /// <param name="htmlName"></param>
        public void ShowHTML(string htmlName)
        {
            try
            {
                string root = "";
                string html = "";
                if (!view.Equals(""))
                {
                    view += "/";
                }
                if (!Config.WebRoot.Equals(""))
                {
                    root = Config.WebRoot + "/";
                }
                string htmlfile = root + Config.template + "/" + view + htmlName + Config.template_type;

                if (pageCacheList.ContainsKey(htmlfile))
                {
                    html = pageCacheList[htmlfile].ToString();
                }
                else
                {
                    if (File.Exists(htmlfile))
                    {
                        html = File.ReadAllText(htmlfile);
                        HTMLParser htmlparser = new HTMLParser(Params);
                        htmlparser.path = view;
                        string tmp = htmlparser.parser(html.ToString());
                        html = tmp;
                        if (!Config.open_debug)
                        {
                            lock (pageCacheList.SyncRoot)
                            {
                                pageCacheList.Add(htmlfile, html);
                            }
                        }
                    }
                    else
                    {
                        html = htmlfile + "模板不存在";
                    }

                }



                Response.StatusCode = 200;
                Response.ContentType = "text/html; charset=" + Config.document_charset;
                Write(html.ToString());
            }
            catch (Exception e)
            {

                Console.Out.WriteLine(e.Message);
            }


        }
        public void ShowStream(byte[] bytes, string ContentType)
        {
            Response.StatusCode = 200;
            Response.ContentType = ContentType + "; charset=" + Config.document_charset;
            WriteByte(bytes);
        }
        /// <summary>
        /// 输出JSON字符串
        /// </summary>
        /// <param name="json">文本</param>
        public void ShowJSON(string json)
        {
            Response.StatusCode = 200;
            Response.ContentType = "application/json; charset=" + Config.document_charset;
            if (Config.JsonToLower)
            {
                var rx = new Regex(@"""(?<v>(\w)*?)"":", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
                var match = rx.Match(json);
                while (match.Success)
                {
                    var originalKeyName = match.Result("${v}");
                    var currentKeyName = originalKeyName.ToString().ToLower();
                    json = rx.Replace(json, "\"" + currentKeyName + "\":", 1, match.Index);
                    match = match.NextMatch();
                }
            }
            Write(json);
        }
        /// <summary>
        /// 输出JSON字符串
        /// </summary>
        /// <param name="value">对象</param>
        public void ShowJSON(object value)
        {
            string json = "";
            try
            {
                if (value == null)
                    value = "";
                json = JsonConvert.SerializeObject(value, new Newtonsoft.Json.JsonSerializerSettings()
                {
                    StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeNonAscii,
                    DateFormatString = "yyyy-MM-dd HH:mm:ss"
                });
                Response.StatusCode = 200;
                Response.ContentType = "application/json; charset=" + Config.document_charset;
            }
            catch (Exception e)
            {

                Console.Out.WriteLine(e.Message);
            }
            if (Config.JsonToLower)
            {
                var rx = new Regex(@"""(?<v>(\w)*?)"":", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
                var match = rx.Match(json);
                while (match.Success)
                {
                    var originalKeyName = match.Result("${v}");
                    var currentKeyName = originalKeyName.ToString().ToLower();
                    json = rx.Replace(json, "\"" + currentKeyName + "\":", 1, match.Index);
                    match = match.NextMatch();
                }
            }
            Write(json);
        }
        /// <summary>
        /// 输出文本
        /// </summary>
        /// <param name="text"></param>
        public void ShowText(string text)
        {
            Response.StatusCode = 200;
            Response.ContentType = "text/html; charset=" + Config.document_charset;
            Write(text);
        }
        private void Write(string content)
        {
            if (content == null)
            {
                content = "";
            }
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            byte[] sdata = null;
            if (compress.CompressType.ToLower().Equals("deflate"))
            {
                sdata = compress.DeflateCompress(buffer);
                Response.AddHeader("Content-Encoding", "deflate");
            }
            else if (compress.CompressType.ToLower().Equals("gzip"))
            {
                Response.AddHeader("Content-Encoding", "gzip");
                sdata = compress.GZipCompress(buffer);
            }
            else
            {
                sdata = buffer;
            }
            Response.ContentEncoding = Encoding.UTF8;
            WriteByte(sdata);
        }
        private void WriteByte(byte[] buffer)
        {
            Response.ContentLength64 = buffer.Length;
            Response.OutputStream.Write(buffer, 0, buffer.Length);
            Response.OutputStream.Close();
            Response.Close();
        }
        public void Success(int code = 0, string message = "操作成功")
        {
            JObject jo = new JObject();
            jo["code"] = code;
            jo["message"] = message;
            ShowJSON(jo);
        }
        public void Fail(int code = -1, string message = "操作失败")
        {
            JObject jo = new JObject();
            jo["code"] = code;
            jo["message"] = message;
            ShowJSON(jo);
        }
        public void Redirect(string url)
        {
            Response.StatusCode = 301;
            Response.RedirectLocation = url;
            Write("");
        }
    }
}
