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
            var str = File.ReadAllText("Assets/buy_Minfin_source.txt");
            str = str.Replace("&nbsp;", "");
            
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(str);
            HtmlNode div = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='au-deals-list']");

            htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(div.OuterHtml);

            var xslt = new XslCompiledTransform();
            var settings = new XsltSettings {EnableScript = true};

            using (StringReader sr = new StringReader(Resource.Minfin_TransformSchema))
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


        public string TrimAll(string trim)
        {
            //Regex.Replace(myString, @"\s+", " ");
            return trim.Trim();
        }
    }
}
