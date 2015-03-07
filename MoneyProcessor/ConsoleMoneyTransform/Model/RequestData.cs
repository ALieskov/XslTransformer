using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMoneyTransform.Model
{
    public class RequestData
    {
        // get currency rate from Privat Api
        public static string GetData(string uri)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";

                var response = (HttpWebResponse)request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var resultString = reader.ReadToEnd();

                //var rates = JsonConvert.DeserializeObject<List<Rate>>(jsonString);

                //return rates.Count > 0 ? rates : new List<Rate>();
                return resultString;
            }
            catch (WebException e)
            {
                var exceptionText = new StreamReader(e.Response.GetResponseStream(), true);
                // TODO: remake for text
                Console.WriteLine(exceptionText);

                //return new List<Rate>();
            }
            return null;
        }
    }
}
