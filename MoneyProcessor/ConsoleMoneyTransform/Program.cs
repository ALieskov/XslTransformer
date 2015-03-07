using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            var str = File.ReadAllText("Assets/buy_FIUA_source.txt");
            str = str.Replace("&nbsp;", "");
            
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(str);
            HtmlNode xslDoBlock = htmlDoc.DocumentNode.SelectSingleNode("//table[@class='local_table local_table-black_market']");

            htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(xslDoBlock.OuterHtml);

            var xslt = new XslCompiledTransform();
            var settings = new XsltSettings {EnableScript = true};

            using (StringReader sr = new StringReader(Resource.Finance_i_ua_TransformSchema))
                using (XmlReader xr = XmlReader.Create(sr))
                {
                    xslt.Load(xr, settings, null);
                }

            StringBuilder resultString = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(resultString);

            xslt.Transform(htmlDoc, writer);
            Console.WriteLine(resultString);
            Console.ReadKey();
        }


        public string Replace(string str, string oldValue, string newValue)
        {
            return str.Replace(oldValue, newValue);
        }
    }
}
