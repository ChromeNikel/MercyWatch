using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mercywatch
{
    class OwerParser : IParser
    {
       private IHtmlDocument doc;
         
        public IHtmlDocument Connect(WebClientEx wc, string playersLink)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
               
            string source = wc.DownloadString(playersLink);
            if (source != null)
            {
                var parser = new HtmlParser();
                return parser.Parse(source);
            }
            else
            {
                return null;
            }
           
        }

        public int GetCompetetiveRate(string competRateSelector, IHtmlDocument connect)
        {
            var div = connect.QuerySelector(competRateSelector);
            if (div != null)
            {
                return Convert.ToInt32(div.TextContent);
            }else
            return 0;
        }

        public Dictionary<string, string> GetHeroRank(string competRateSelector, string nameHeroSelector, IHtmlDocument connect)
        {
            var winDiv = connect.QuerySelectorAll(competRateSelector);
            var nameHero = connect.QuerySelectorAll(nameHeroSelector);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (winDiv.Length != 0 && nameHero.Length != 0)
            {          
                for (int i = 0; i < winDiv.Length; i++)
                {
                    string win = winDiv[i].TextContent;
                    string name = nameHero[i].TextContent;
                    if (!win.Contains('%'))
                    {
                        win = Convert.ToChar(8734).ToString();
                    }
                    byte[] bytes0 = Encoding.Default.GetBytes(name);
                    name = Encoding.UTF8.GetString(bytes0);
                    dic[name] = win;
                }               
            }
            else
            {
                dic["Нет достпа."] = "Закрыт copmetitive";
                dic["Нет достпа.."] = "Закрыт copmetitive";
                dic["Нет достпа..."] = "Закрыт copmetitive";
            }
                return dic;

        }

        public string GetName(string competRateSelector, IHtmlDocument connect)
        {
            var div = connect.QuerySelector(competRateSelector);
            if (div != null)
            {
                div = div.QuerySelector("h1");
                string name = div.TextContent;
                name = name.Substring(0, name.IndexOf('#'));
                byte[] bytes0 = Encoding.Default.GetBytes(name);
                name = Encoding.UTF8.GetString(bytes0);
                return name;
            }
            else
            {
                return "Закрыт competitive";
            }
        }

        public string GetTag(string competRateSelector, IHtmlDocument connect)
        {
            var div = connect.QuerySelector(competRateSelector);
            if (div != null)
            {
                div = div.QuerySelector("h1");
                string tag = div.TextContent;
                tag = tag.Substring(tag.IndexOf('#'), tag.Length - tag.IndexOf('#') - 2);
                return tag;
            }
            else
            {
                return "Закрыт competitive";
            }
           
        }

        public float GetWinRate(string competRateSelector, IHtmlDocument connect)
        {
            var div = connect.QuerySelector(competRateSelector);
            if (div != null)
            {
                div = div.QuerySelector("dd");
                string winRate = div.TextContent;
                winRate = winRate.Substring(0, winRate.Length - 1);
                winRate = winRate.Replace('.', ',');
                return (float)Convert.ToDouble(winRate);
            }
            else
            {
                return 0;
            }          
        }

        public string[] getTeamFromVK(string competRateSelector, IHtmlDocument connect)
        {
            var div = connect.QuerySelectorAll(competRateSelector);
           
            return null;
        }
    }
}
