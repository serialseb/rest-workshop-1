using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace OpenDocuments.Client
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Please enter the author");
            string author = Console.ReadLine();

            Console.WriteLine("Please enter file content");
            string fileContent = Console.ReadLine();

            var request = (HttpWebRequest)WebRequest.Create(@"http://127.0.0.1.:6666/documents");
            request.Method = "POST";
            request.Accept = "*/*";
            request.ContentType = "multipart/form-data;boundary=sunnyday";
            request.Proxy = WebRequest.GetSystemWebProxy();
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
    }
}
