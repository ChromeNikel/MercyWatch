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
            try
            {
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
            catch (System.Net.WebException e)
            {
                try
                {
                    string start = "https://www.overbuff.com/players/pc/";
                    string afterTag = playersLink.Substring(playersLink.IndexOf('%'));
                    int lenName = playersLink.Length - start.Length - afterTag.Length;
                    string name = playersLink.Substring(start.Length, lenName);
                    name = name[0].ToString().ToUpper() + name.Substring(1);
                    playersLink = start + name + afterTag;
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
                catch (System.Net.WebException ee)
                {
                    try
                    {
                        string start = "https://www.overbuff.com/players/pc/";
                        string afterTag = playersLink.Substring(playersLink.IndexOf('%'));
                        int lenName = playersLink.Length - start.Length - afterTag.Length;
                        string name = playersLink.Substring(start.Length, lenName);
                        name = name.ToUpper();
                        playersLink = start + name + afterTag;
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
                    catch (Exception eee) { return null; }
                }
            }          
        }

        public int GetCompetetiveRate(string competRateSelector, IHtmlDocument connect)
        {
            if (connect != null)
            {
                var div = connect.QuerySelector(competRateSelector);
                if (div != null)
                {
                    return Convert.ToInt32(div.TextContent);
                }
                else
                    return 0;
            }
            else return 0;           
        }

        public Dictionary<string, string> GetHeroRank(string competRateSelector, string nameHeroSelector, IHtmlDocument connect)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (connect != null)
            {
                var winDiv = connect.QuerySelectorAll(competRateSelector);
                var nameHero = connect.QuerySelectorAll(nameHeroSelector);               
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
                    dic["Нет доступа."] = "Закрыт copmetitive";
                    dic["Нет доступа.."] = "Закрыт copmetitive";
                    dic["Нет доступа..."] = "Закрыт copmetitive";
                }                
            }
            else
            {
                dic["Нет доступа."] = "Закрыт copmetitive";
                dic["Нет доступа.."] = "Закрыт copmetitive";
                dic["Нет доступа..."] = "Закрыт copmetitive";
            }
            return dic;

        }

        public string GetName(string competRateSelector, IHtmlDocument connect)
        {
            if (connect != null)
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
            else
            {
                return "Неверные данные";
            }
        }

        public string GetTag(string competRateSelector, IHtmlDocument connect)
        {
            if (connect != null)
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
            else
            {
                return "Неверные данные";
            }
           
           
        }

        public float GetWinRate(string competRateSelector, IHtmlDocument connect)
        {
            if (connect != null)
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
            else { return 0; }
	{
                   
                }
            }
           
    }
}
