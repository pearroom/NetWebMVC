/*苏兴迎 E-Mail:284238436@qq.com*/
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace MVC.Command
{
    public class Command
    {
        public static JArray MIME;
        public static JObject configFile;
        private static Assembly assembly = Assembly.LoadFrom(Config.AppExe);
        public static Compress compress = new Compress();
        public void RunRoule(object _context_, RouleMap roule, Interceptor interceptor_)
        {
            HttpListenerContext context = _context_ as HttpListenerContext;
            try
            {

                string path = context.Request.RawUrl.Trim();
                path = Uri.UnescapeDataString(path);
                string url = path;
                string methodname = "";
                string url_suffix = "";
                if (isMimeType(path) == null)
                {
                    string[] m = path.Split('/');

                    RouleItem action = null;

                    action = roule.getRoule(path, ref methodname, ref url_suffix);
                    if (action != null)
                    {
                        HttpSession Session = null;
                        bool isInterceptor = false;
                        if (Config.Session_open)
                        {
                            Session = new HttpSession(context);
                        }
                        if (action.isInterceptor)
                        {
                            Type Interceptor_type = Type.GetType(interceptor_.ToString());
                            object Interceptor_obj = Activator.CreateInstance(Interceptor_type);
                            MethodInfo Interceptor_method = Interceptor_type.GetMethod("Init", new Type[] { typeof(HttpListenerContext), typeof(HttpSession) });
                            object[] parameters1 = new object[] { context, Session };
                            Interceptor_method.Invoke(Interceptor_obj, parameters1);
                            Interceptor_method = Interceptor_type.GetMethod("isInterceptor");
                            isInterceptor = (bool)Interceptor_method.Invoke(Interceptor_obj, null);
                        }

                        if (!isInterceptor)
                        {

                            object obj = assembly.CreateInstance(action.action); // 创建类的实例 
                            Type type = obj.GetType();
                            MethodInfo method = type.GetMethod("Init", new Type[] { typeof(HttpListenerContext), typeof(HttpSession), typeof(string), typeof(string), typeof(Compress) });
                            object[] parameters = new object[] { context, Session, action.view, url_suffix, compress };
                            method.Invoke(obj, parameters);
                            method = type.GetMethod(methodname);
                            if (method != null)
                            {
                                object[] parameters1 = formatParam(_context_, method, url_suffix);
                                method.Invoke(obj, parameters1);
                                if (context.Response.ContentLength64 == 0)
                                {
                                    Error404(context, url);
                                }
                            }
                            else
                            {
                                Error404(context, url);
                            }
                        }

                    }
                    else
                    {
                        Error404(context, url);
                    }

                }
                else
                {
                    if (Config.open_cache)
                    {

                        string htmlfile = System.IO.Directory.GetCurrentDirectory() + path;
                        if (htmlfile.IndexOf('?') > 0)
                        {
                            htmlfile = htmlfile.Substring(0, htmlfile.IndexOf('?'));
                        }
                        if (File.Exists(htmlfile))
                        {
                            DateTime n = DateTime.Now;
                            string Modified_time = n.ToUniversalTime().ToString("r");
                            string Expires_time = n.AddHours(24).ToUniversalTime().ToString("r");
                            context.Response.AddHeader("Cache-Control", "max-age=" + Config.cache_max_age);
                            context.Response.AddHeader("Pragma", "Pragma");
                            context.Response.AddHeader("Last-Modified", Modified_time);
                            context.Response.AddHeader("Expires", Expires_time);

                            string extension = System.IO.Path.GetExtension(htmlfile).Replace(".", "");
                            byte[] data = File.ReadAllBytes(htmlfile);
                            string mime = getMimeType(extension);
                            context.Response.ContentType = mime;
                            context.Response.StatusCode = 200;
                            WriteByte(context, data);
                        }
                        else
                        {
                            Error404(context, path);
                        }
                    }
                    else
                    {
                        context.Response.AddHeader("Cache-Control", "no-cache,no-store");
                    }



                }
            }
            catch (Exception e)
            {
                Error500(context, e.Message);
            }

        }
        private object[] formatParam(object _context_, MethodInfo method, string url_suffix)
        {
            HttpListenerContext context = _context_ as HttpListenerContext;
            ParameterInfo[] parames = method.GetParameters();
            object[] parameters = new object[parames.Length];
            string[] urlparams = null;
            if (context.Request.HttpMethod.Equals("GET"))
            {
                if (url_suffix.IndexOf('?') > 0 && parames.Length > 0)
                {
                    url_suffix = url_suffix.Substring(url_suffix.IndexOf('?') + 1);
                    urlparams = url_suffix.Split('&');
                    //  if (urlparams.Length == parames.Length)
                    {
                        foreach (string paramv in urlparams)
                        {
                            string[] ss = paramv.Split('=');
                            for (int i = 0; i < parames.Length; i++)
                            {
                                if (parames[i].Name.Equals(ss[0]))
                                {
                                    string name = parames[i].Name;
                                    Type paramType = parames[i].ParameterType;
                                    parameters[i] = getParamValue(paramType, ss[1]);
                                    break;
                                }
                            }
                        }

                    }
                }
                else
                {
                    if (parames.Length > 0)
                    {
                        urlparams = url_suffix.Split('/');
                        //  if (urlparams.Length == parames.Length + 1)
                        {

                            for (int i = 0; i < parames.Length; i++)
                            {
                                if (urlparams.Length > 1 && i < urlparams.Length - 1)
                                {
                                    string name = parames[i].Name;
                                    Type paramType = parames[i].ParameterType;
                                    parameters[i] = getParamValue(paramType, urlparams[i + 1]);
                                }


                            }
                        }
                    }
                }
            }
            return parameters;
        }
        private object getParamValue(Type paramType, string value)
        {
            object ret = null;
            if (paramType.Name.Equals("String"))
            {
                ret = value;
            }
            else if (paramType.Name.Equals("Int32"))
            {
                try
                {
                    ret = Convert.ToInt32(value);
                }
                catch (Exception)
                {

                    ret = Convert.ToInt32("0");
                }
            }
            else if (paramType.Name.Equals("Int16"))
            {
                try
                {
                    ret = Convert.ToInt16(value);
                }
                catch (Exception)
                {

                    ret = Convert.ToInt32("0");
                }
            }
            else if (paramType.Name.Equals("Int64"))
            {
                try
                {
                    ret = Convert.ToInt64(value);
                }
                catch (Exception)
                {

                    ret = Convert.ToInt32("0");
                }
            }
            else if (paramType.Name.Equals("Double"))
            {
                try
                {
                    ret = Convert.ToDouble(value);
                }
                catch (Exception)
                {

                    ret = Convert.ToDouble(0);
                }
            }
            else if (paramType.Name.Equals("DateTime"))
            {
                try
                {
                    ret = Convert.ToDateTime(value);
                }
                catch (Exception)
                {

                    ret = null;
                }
            }
            else if (paramType.Name.Equals("Single"))
            {
                try
                {
                    ret = Convert.ToSingle(value);
                }
                catch (Exception)
                {

                    ret = Convert.ToSingle(0);
                }
            }
            else if (paramType.Name.Equals("float"))
            {
                try
                {
                    ret = Convert.ToSingle(value);
                }
                catch (Exception)
                {

                    ret = Convert.ToSingle(0);
                }
            }

            return ret;
        }
        public void Write(HttpListenerContext context_, string content)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            WriteByte(context_, buffer);
        }
        private static void WriteByte(HttpListenerContext context_, byte[] buffer)
        {

            byte[] sdata = null;
            if (compress.CompressType.ToLower().Equals("deflate"))
            {
                context_.Response.AddHeader("Content-Encoding", "deflate");
                sdata = compress.DeflateCompress(buffer);
            }
            else if (compress.CompressType.ToLower().Equals("gzip"))
            {
                context_.Response.AddHeader("Content-Encoding", "gzip");
                sdata = compress.GZipCompress(buffer);
            }
            else
            {
                sdata = buffer;
            }
            context_.Response.ContentLength64 = sdata.Length;
            context_.Response.OutputStream.Write(sdata, 0, sdata.Length);
            context_.Response.OutputStream.Close();
            context_.Response.Close();
        }
        private void Error404(HttpListenerContext context_, string url)
        {
            context_.Response.StatusCode = 404;
            context_.Response.ContentType = "text/html; charset=" + Config.document_charset;
            string s = "<html><body><div style=\"text-align: left;\">";
            s = s + "<div><h1>Error 404</h1></div>";
            s = s + "<hr><div>[ " + url + " ] Not Find Page </div></div></body></html>";
            Write(context_, s);
        }
        private void Error500(HttpListenerContext context_, string Error)
        {
            context_.Response.StatusCode = 500;
            context_.Response.ContentType = "text/html; charset=" + Config.document_charset;
            string s = "<html><body><div style=\"text-align: left;\">";
            s = s + "<div><h1>Error 500</h1></div>";
            s = s + "<hr><div> " + Error + " </div></div></body></html>";
            Write(context_, s);
        }
        public static bool readConifg()
        {
            string file = Config.RootPath + "/" + Config.config_file;
            StringBuilder html = new StringBuilder();
            if (File.Exists(file))
            {
                try
                {
                    html.Append(File.ReadAllText(file));
                    configFile = JObject.Parse(html.ToString());
                    compress.CompressType = configFile["Server"]["Compress"].ToString();
                }
                catch (Exception)
                {

                    return false;
                }

                return true;
            }
            else
                return false;
        }
        public static bool readMime()
        {
            string mimefile = Config.RootPath + "/" + Config.mime_file;
            StringBuilder html = new StringBuilder();
            if (File.Exists(mimefile))
            {
                try
                {
                    html.Append(File.ReadAllText(mimefile));
                    MIME = JArray.Parse(html.ToString());
                }
                catch (Exception)
                {

                    return false;
                }

                return true;
            }
            else
                return false;

        }
        public static string getMimeType(string type)
        {
            foreach (var jo in MIME)
            {
                if (type.ToLower().Equals(jo["Extensions"].ToString().ToLower()))
                {
                    return jo["MimeType"].ToString() + "; charset=utf-8";
                }
            }
            return "";
        }
        public static string isMimeType(string path)
        {
            foreach (var jo in MIME)
            {
                if (path.ToLower().IndexOf("." + jo["Extensions"].ToString().ToLower()) > 0)
                {
                    return jo["MimeType"].ToString() + "; charset=utf-8";
                }
            }
            return null;
        }
    }

}