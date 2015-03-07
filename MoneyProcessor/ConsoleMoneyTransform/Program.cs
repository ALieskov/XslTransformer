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
            var str = File.ReadAllText("Assets/sell_FIUA_source.txt");
            str = str.Replace("&nbsp;", "");

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(str);
            HtmlNode table = htmlDoc.DocumentNode.SelectSingleNode("//table[@class='local_table local_table-black_market']");

            htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(table.OuterHtml);

            var xslt = new XslCompiledTransform();
            using (StringReader sr = new StringReader(Resource.Finance_i_ua_TransformSchema))
                using (XmlReader xr = XmlReader.Create(sr))
                {
                    xslt.Load(xr);
                }

            StringBuilder resultString = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(resultString);
            
            xslt.Transform(htmlDoc, writer);
            Console.WriteLine(resultString);
            Console.ReadKey();
        }
    }
}
