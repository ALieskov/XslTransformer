using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;
using ConsoleMoneyTransform.Abstractions;
using HtmlAgilityPack;

namespace ConsoleMoneyTransform.Model
{
    public class FinanceTransformer : ITransformer
    {
        public MoneyOrders GetTransformedResult()
        {
            throw new NotImplementedException();
        }

        void Transform()
        {
            var str = File.ReadAllText("Assets/all_finance_source.txt");
            //var str = RequestData.GetData(URLConstants.Finance);
            str = str.Replace("&nbsp;", "");
            str = str.Replace("<br>", "");
            str = str.Replace("Куплю", "buy");

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(str);
            HtmlNode xslDoBlock = htmlDoc.DocumentNode.SelectSingleNode("//tbody[@aria-live='polite']");

            htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(xslDoBlock.OuterHtml);

            var xslt = new XslCompiledTransform();
            var settings = new XsltSettings { EnableScript = true };

            using (StringReader sr = new StringReader(Resource.Finance_TransformSchema))
            using (XmlReader xr = XmlReader.Create(sr))
            {
                xslt.Load(xr, settings, null);
            }

            var resultString = new StringBuilder();
            var writer = XmlWriter.Create(resultString);


            var xslArg = new XsltArgumentList();
            xslArg.AddParam("neededOperation", "", "buy");
            xslArg.AddParam("intrestedCurrencyName", "", "EUR");

            xslt.Transform(htmlDoc, xslArg, writer);

            Console.WriteLine(resultString);
            Console.ReadKey();
        }

    }

}
