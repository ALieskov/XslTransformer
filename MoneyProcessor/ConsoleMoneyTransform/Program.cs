using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;
using HtmlAgilityPack;

namespace ConsoleMoneyTransform
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = File.ReadAllText("source.txt");
            //str = str.Replace("&nbsp;", "");

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(str);

            HtmlNode div = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='au-deals-list']");
            htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(div.OuterHtml);

            XslTransform xslt = new XslTransform();
            xslt.Load("infoBuilder.xsl");
            xslt.Transform(htmlDoc, null, Console.Out, null);
            Console.ReadKey();
        }
    }
}
