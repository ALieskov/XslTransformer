using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace ConsoleMoneyTransform
{
    class Program
    {
        static void Main(string[] args)
        {
            //var myXslTransform = new XslTransform();
            //myXslTransform.Load("infoBuilder.xsl");
            //myXslTransform.Transform("source.txt", "destination.xml");
            var str = File.ReadAllText("source.txt");
            str = str.Replace("&nbsp;", "");

            XmlDocument doc = new XmlDocument();

            using (var reader = new StringReader(str))
            {
                doc.Load(reader);
            }

            XslTransform xslt = new XslTransform();
            xslt.Load("infoBuilder.xsl");
            
            // Create a new document containing just the node fragment.
            XmlNode testNode = doc.DocumentElement.FirstChild;
            XmlDocument tmpDoc = new XmlDocument();
            tmpDoc.LoadXml(testNode.OuterXml);
            // Pass the document containing the node fragment 
            // to the Transform method.
            //Console.WriteLine("Passing " + tmpDoc.OuterXml + " to print_root.xsl");
            xslt.Transform(tmpDoc, null, Console.Out, null);
            Console.ReadKey();
        }
    }
}
