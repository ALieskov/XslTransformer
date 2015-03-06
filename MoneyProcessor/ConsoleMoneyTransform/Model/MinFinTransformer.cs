﻿using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using ConsoleMoneyTransform.Abstractions;
using HtmlAgilityPack;

namespace ConsoleMoneyTransform.Model
{
    public class MinFinTransformer: ITransformer
    {
        public MoneyOrders GetTransformedResult()
        {
            throw new NotImplementedException();
        }

        void Transform()
        {
            var str = File.ReadAllText("Assets/buy_source.txt");
            str = str.Replace("&nbsp;", "");

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(str);
            HtmlNode div = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='au-deals-list']");

            htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(div.OuterHtml);

            var xslt = new XslCompiledTransform();
            using (StringReader sr = new StringReader(Resource.MinfinTransformSchema))
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
