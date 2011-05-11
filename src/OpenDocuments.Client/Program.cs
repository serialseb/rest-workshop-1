using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenDocuments.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string entryBookmark = @"http://127.0.0.1.:6666/home";

            var link = DiscoverLinkFromHeaders(entryBookmark);

            var uriData = link.FirstOrDefault(x => x.Value["rel"].Contains("\"http:/rels.openwrap.org/OpenDoc/index\"")).Key.Trim();
            var uri = new Uri(new Uri(entryBookmark, UriKind.Absolute),
                              new Uri(uriData.Substring(1, uriData.Length - 2), UriKind.RelativeOrAbsolute));
            Console.WriteLine("Please enter the author");
            string author = Console.ReadLine();

            Console.WriteLine("Please enter file content");
            string fileContent = Console.ReadLine();

            HttpWebRequest request = CreateRequest(uri, "POST");
            request.Accept = "*/*";
            request.ContentType = "multipart/form-data;boundary=sunnyday";
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.WriteLine();
                writer.WriteLine("--sunnyday");
                writer.WriteLine("Content-Disposition: form-data; name=\"author\"");
                writer.WriteLine("Content-Type: text/plain");
                writer.WriteLine();
                writer.WriteLine(author);
                writer.WriteLine("--sunnyday");
                writer.WriteLine("Content-Disposition: form-data; name=\"sentFile\"; filename=\"jonny.txt\"");
                writer.WriteLine("Content-Type: application/octet-stream");
                writer.WriteLine();
                writer.WriteLine(fileContent);
                writer.WriteLine("--sunnyday--");
            }

            try
            {
                Console.WriteLine("submitting request ...");
                var response = request.GetResponse();
                var responseStream = response.GetResponseStream();
                if (responseStream == null)
                {
                    Console.WriteLine("Response is null");
                    return;
                }
                var stringBuilder = new StringBuilder();
                var buf = new byte[8192];
                int count;
                do
                {
                    count = responseStream.Read(buf, 0, buf.Length);
                    if (count == 0) continue;
                    var tempString = Encoding.ASCII.GetString(buf, 0, count);
                    stringBuilder.Append(tempString);
                }
                while (count > 0);
                Console.WriteLine(stringBuilder.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
            return request;
        }

        private static IDictionary<string, ILookup<string, string>> DiscoverLinkFromHeaders(string entryBookmark)
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
