using AngleSharp.Dom.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercywatch
{
    interface IParser
    {
        IHtmlDocument Connect(WebClientEx wc, string playersLink);

        int GetCompetetiveRate(string competRateSelector, IHtmlDocument connect);

        string GetName(string competRateSelector, IHtmlDocument connect);

        string GetTag(string competRateSelector, IHtmlDocument connect);

        float GetWinRate(string competRateSelector, IHtmlDocument connect);

        Dictionary<string, string> GetHeroRank(string competRateSelector, string nameHeroSelector, IHtmlDocument connect);
    }
}
