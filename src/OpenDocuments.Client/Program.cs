using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace OpenDocuments.Client
{
    class Program
    {
        private static string DEFAULT_NS = "http://schemas.datacontract.org/2004/07/Open.Documents";

        static void Main(string[] args)
        {
            Console.WriteLine("Enter address - defaults to http://127.0.0.1.:6666/home");

            var consoleLine = Console.ReadLine();
            string entryBookmark = string.IsNullOrEmpty(consoleLine)
                    ? @"http://127.0.0.1.:6666/home"
                    : consoleLine;

            var link = DiscoverLinkFromHeaders(entryBookmark);

            var uriData = link.FirstOrDefault(x => x.Value["rel"].Contains("\"http:/rels.openwrap.org/OpenDoc/index\"")).Key.Trim();
            var uri = new Uri(new Uri(entryBookmark, UriKind.Absolute),
                              new Uri(uriData.Substring(1, uriData.Length - 2), UriKind.RelativeOrAbsolute));
            
Console.WriteLine("Please enter the author");
            string author = Console.ReadLine();

            Console.WriteLine("Please enter file content");
            string fileContent = Console.ReadLine();



            HttpWebRequest request = CreateRequest(uri, "POST");


            request.ContentType = "application/xml";
            var document =
                new MemoryStream(Encoding.UTF8.GetBytes(@"
<DocumentInfo xmlns=""http://schemas.datacontract.org/2004/07/Open.Documents"">
            <Author>" + author + @"</Author>
            <FileName>Filename.txt</FileName>
</DocumentInfo>"));
            document.CopyTo(request.GetRequestStream());

            var response = request.GetResponse();
            var responseDoc = XDocument.Load(response.GetResponseStream());

            var binaryHref = responseDoc.Descendants(XName.Get("DataHref", DEFAULT_NS))
                .First().Value;

            var binaryRequest = CreateRequest(binaryHref, "PUT");
            binaryRequest.ContentType = "application/octet-stream";
            new MemoryStream(Encoding.UTF8.GetBytes(fileContent)).CopyTo(binaryRequest.GetRequestStream());
            var binaryResponse = (HttpWebResponse)binaryRequest.GetResponse();
            if (binaryResponse.StatusCode == HttpStatusCode.Created)
            {
                Console.WriteLine("File has been uploaded.");
            }
            else if (binaryResponse.StatusCode == HttpStatusCode.OK)
                Console.WriteLine("File was updated.");

            Console.ReadLine();
        }

        private static HttpWebRequest CreateRequest(string uri, string method)
        {
            return CreateRequest(new Uri(uri, UriKind.RelativeOrAbsolute), method);
        }
        private static HttpWebRequest CreateRequest(Uri uri, string method)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Proxy = WebRequest.GetSystemWebProxy();
            request.Method = method;

            request.Accept = "application/xml, */*;q=0.5";
            return request;
        }

        private static IEnumerable<KeyValuePair<string, ILookup<string, string>>> DiscoverLinkFromHeaders(string entryBookmark)
        {
            var headers = CreateRequest(entryBookmark, "GET").GetResponse().Headers["Link"]
                .Split(new[]{","}, StringSplitOptions.RemoveEmptyEntries);
            return (from link in headers
                    let components = link.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries)
                    let uri = components.First()
                    let parameters = (from keyValue in components.Skip(1)
                                      let key = keyValue.Substring(0, keyValue.IndexOf("="))
                                      select
                                          new
                                              {
                                                  key,
                                                  value = keyValue.Substring(keyValue.IndexOf("=") + 1)
                                              }
                                     ).ToLookup(x => x.key, x => x.value)
                    select new {uri, parameters}).ToDictionary(x => x.uri, x => x.parameters);
            ;

        }
    }
}
