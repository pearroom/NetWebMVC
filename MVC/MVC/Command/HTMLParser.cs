/*苏兴迎 E-Mail:284238436@qq.com*/
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace MVC.Command
{
    public class HTMLParser
    {
        Hashtable Params_;
        public string path { get; set; }
        public HTMLParser(Hashtable Params)
        {
            Params_ = Params;
        }
        public string parser(string html)
        {
            string content = "";
            content = html;
            content = parserInclude(content);
            content = parserApp(content);
            foreach (string key in Params_.Keys)
            {
                object value = Params_[key];
                if (value.GetType() == typeof(string))
                {
                    content = this.parserText(key, (string)value, content);
                }
                else
                {
                    if (value is IList)
                    {
                        content = parserList(key, value, content);
                    }
                    else
                    {
                        content = parserObject(key, value, content);
                    }

                }
            }
            return content;
        }
        private string parserApp(string html)
        {

            string expr = "__APP__";
            string value = "";
            if (string.IsNullOrEmpty(Config.AppName.Trim()))
            {
                value = "";
            }
            else
            {
                value = "/" + Config.AppName;
            }
            MatchCollection mc = Regex.Matches(html, expr);
            foreach (Match m in mc)
            {
                html = Regex.Replace(html, m.Value, value);
            }


            return html;
        }
        private string parserText(string key, string value, string html)
        {
            string expr = "#{" + key + "}";
            MatchCollection mc = Regex.Matches(html, expr);
            foreach (Match m in mc)
            {
                html = Regex.Replace(html, m.Value, value);
            }
            return html;
        }
        private string parserList(string key, object value, string html)
        {
            string expr = "<#list.*data=" + key + " [\\s\\S]*?</#list>";
            MatchCollection mc = Regex.Matches(html, expr);
            foreach (Match m in mc)
            {
                string temp = Regex.Match(m.Value.ToString(), "<#list.*?>").Value.Trim();

                string itemvalue = temp.Replace("<#list", "").Replace("data=", "").Replace(key, "").Replace("item=", "");
                itemvalue = itemvalue.Replace(">", "").Replace("/", "").Trim();
                string content = m.Value;
                content = Regex.Replace(content, "<#list.*?>", "");
                content = Regex.Replace(content, "</#list>", "");
                StringBuilder text = new StringBuilder();
                foreach (var obj in (IEnumerable)value)
                {
                    string txt = parserObject(itemvalue, obj, content);
                    text.Append(txt);
                }

                html = Regex.Replace(html, m.Value, text.ToString());

            }


            return html;
        }
        private string parserObject(string key, object value, string html)
        {
            PropertyInfo[] propertyInfos = value.GetType().GetProperties();
            foreach (var item in propertyInfos)
            {

                string expr = "#{" + key + "." + item.Name + "}";
                MatchCollection mc = Regex.Matches(html, expr);
                foreach (Match m in mc)
                {
                    string v = (item.GetValue(value, null) == null ? "" : item.GetValue(value, null)).ToString();
                    html = Regex.Replace(html, m.Value, v);

                }

            }

            return html;
        }
        private string parserInclude(string html)
        {
            string expr = "<#include.*file=[\\s\\S]*?\\>";
            MatchCollection mc = Regex.Matches(html, expr);
            foreach (Match m in mc)
            {
                string htmlfile = m.Value;
                htmlfile = htmlfile.Replace("<#include", "").Replace("file=", "").Replace("/>", "").Replace("\"", "").Trim();

                string value = LoadHtml(htmlfile);
                html = Regex.Replace(html, m.Value, value);
            }
            return html;
        }
        private string LoadHtml(string htmlfile)
        {
            string file = "";
            string root = "";
            if (!Config.WebRoot.Equals(""))
            {
                root = Config.WebRoot + "/";
            }
            if (htmlfile[0] == '/')
            {

                file = root + Config.template + htmlfile;
            }
            else
            {
                file = root + Config.template+"/"+path + htmlfile;
            }

            StringBuilder html = new StringBuilder();
            if (File.Exists(file))
            {
                try
                {
                    html.Append(File.ReadAllText(file));
                    return html.ToString();

                }
                catch (Exception)
                {

                    return "";
                }
            }
            else
                return "";
        }
    }
}
