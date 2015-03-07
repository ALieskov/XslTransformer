using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using ConsoleMoneyTransform.Helper;
using ConsoleMoneyTransform.Model;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace ConsoleMoneyTransform
{
    class Program
    {
        static void Main(string[] args)
        {



            var web = new HtmlWeb();
            web = new HtmlWeb
            {
                AutoDetectEncoding = true,
                //OverrideEncoding = Encoding.GetEncoding("windows-1251"),
            };
            var document = web.Load(URLConstants.FinanceIUABuy);
            var page = document.DocumentNode;

            /*
            try
            {
                // download each page and dump the content
                var task = MessageLoopWorker.Run(DoWorkAsync, URLConstants.Finance);
                task.Wait();
                Console.WriteLine("DoWorkAsync completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("DoWorkAsync failed: " + ex.Message);
            }
            /**/

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
            var settings = new XsltSettings {EnableScript = true};

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
            
            Console.WriteLine(" ");
            Console.WriteLine("Press Enter to exit.");
            Console.ReadKey();
        }



        // navigate WebBrowser to the list of urls in a loop
        static async Task<object> DoWorkAsync(object[] args)
        {
            Console.WriteLine("Start working.");

            using (var wb = new WebBrowser())
            {
                wb.ScriptErrorsSuppressed = true;

                TaskCompletionSource<bool> tcs = null;
                WebBrowserDocumentCompletedEventHandler documentCompletedHandler = (s, e) =>
                    tcs.TrySetResult(true);

                // navigate to each URL in the list
                foreach (var url in args)
                {
                    tcs = new TaskCompletionSource<bool>();
                    wb.DocumentCompleted += documentCompletedHandler;
                    try
                    {
                        wb.Navigate(url.ToString());
                        // await for DocumentCompleted
                        await tcs.Task;
                    }
                    finally
                    {
                        wb.DocumentCompleted -= documentCompletedHandler;
                    }
                    // the DOM is ready
                    Console.WriteLine(url.ToString());
                    Console.WriteLine(wb.Document.Body.OuterHtml);
                }
            }

            Console.WriteLine("End working.");
            return null;
        }

    }

    // a helper class to start the message loop and execute an asynchronous task
    public static class MessageLoopWorker
    {
        public static async Task<object> Run(Func<object[], Task<object>> worker, params object[] args)
        {
            var tcs = new TaskCompletionSource<object>();

            var thread = new Thread(() =>
            {
                EventHandler idleHandler = null;

                idleHandler = async (s, e) =>
                {
                    // handle Application.Idle just once
                    Application.Idle -= idleHandler;

                    // return to the message loop
                    await Task.Yield();

                    // and continue asynchronously
                    // propogate the result or exception
                    try
                    {
                        var result = await worker(args);
                        tcs.SetResult(result);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }

                    // signal to exit the message loop
                    // Application.Run will exit at this point
                    Application.ExitThread();
                };

                // handle Application.Idle just once
                // to make sure we're inside the message loop
                // and SynchronizationContext has been correctly installed
                Application.Idle += idleHandler;
                Application.Run();
            });

            // set STA model for the new thread
            thread.SetApartmentState(ApartmentState.STA);

            // start the thread and await for the task
            thread.Start();
            try
            {
                return await tcs.Task;
            }
            finally
            {
                thread.Join();
            }
        }
    }
}
