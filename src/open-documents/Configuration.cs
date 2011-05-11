using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.Serialization;
using Open.Documents.Resources;
using OpenRasta.Configuration;
using OpenRasta.IO;
using OpenRasta.Web;

namespace Open.Documents
{
    public class Configuration : IConfigurationSource
    {
        public void Configure()
        {
            // for 2.1:
            // ResourceSpace.Uses.UriDecorator<ContentTypeExtensionUriDecorator>();
            ResourceSpace.Uses.ConventionsFrom(this);
            ResourceSpace.Has.Resource<Home>()
                .Anywhere
                .RenderedByAspx("~/Views/Home.aspx");
            ResourceSpace.Has.Resource<ContactUs>()
                .Uri("/contact-us?email={email}&comment={comment}")
                .And.Uri("/contact-us-empty").Named("empty")
                .And.Uri("/contact-us/{email}/{comment}");




            ResourceSpace.Has.Resource<DocumentInfo>()
                .Uri("/documents/{id}")
                .Handler<DocumentInfoHandler>()
                .XmlDataContract().And
                .JsonDataContract().And
                .RenderedByAspx("~/Views/DocumentInfo.aspx")
                .MediaType(new MediaType("application/xhtml+xml;q=1"))
                .MediaType(new MediaType("text/html;q=0.9")).Extension("acme");



            ResourceSpace.Has.Resource<DocumentData>()
                .Uri("/documents/{id}/raw")
                .Handler<DocumentDataHandler>();

            ResourceSpace.Has.Resource<DocumentLibrary>()
                .Uri("/documents")
                .Handler<DocumentLibraryHandler>()
                .RenderedByAspx("~/Views/Documents.aspx");

        }
    }

    public class DocumentLibraryHandler
    {
        public DocumentLibrary Get()
        {
            return Database.Documents;
        }
        public OperationResult Post(DocumentInfo doc, IFile sentFile)
        {
            doc.Id = Database.Documents.Max(x => x.Id) + 1;
            doc.FileName = sentFile.FileName;
            using (var reader = new BinaryReader(sentFile.OpenStream()))
                doc.Data = reader.ReadBytes((int)sentFile.Length);
            doc.LastModifiedTimeUTC = DateTime.UtcNow;
            doc.DataHref = new DocumentData { Id = doc.Id }.CreateUri();
            var createdUri = doc.CreateUri();
            Database.Documents.Add(doc);

            return new OperationResult.Created
            {
                RedirectLocation = createdUri,
                ResponseResource = "Document created at " + createdUri
            };
        }

    }

    public class DocumentLibrary : Collection<DocumentInfo>
    {
        
    }

    public static class Database
    {
        static Database()
        {
            Documents = new DocumentLibrary
                            {
                                new DocumentInfo
                                    {
                                        Id = 1,
                                        Author = "@serialseb",
                                        Data = ReadNotepad(),
                                        DataHref = new DocumentData {Id = 1}.CreateUri(),
                                        LastModifiedTimeUTC = DateTime.UtcNow,
                                        FileName = "Notepad"
                                    }
                            };

        }

        private static byte[] ReadNotepad()
        {
            using (var stream = File.OpenRead(@"c:\windows\notepad.exe"))
            using (var reader = new BinaryReader(stream))
                return reader.ReadBytes((int) stream.Length);
        }

        public static DocumentLibrary Documents { get; set; }
    }
    public class DocumentDataHandler
    {
        public IFile Get(int id)
        {
            var doc = Database.Documents.FirstOrDefault(x => x.Id == id);
            var rawData = doc.Data;
            return new InMemoryFile(new MemoryStream(rawData))
                       {
                           ContentType = MediaType.ApplicationOctetStream,
                           FileName = doc.FileName
                       };
        }
    }

    public class DocumentInfoHandler
    {
        public DocumentInfo Get(int id)
        {
            return Database.Documents.FirstOrDefault(x => x.Id == id);
        }
    }

    public class DocumentData
    {
        public int Id { get; set; }
    }

    public class DocumentInfo
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public Uri DataHref { get; set; }
        [IgnoreDataMember]
        public byte[] Data { get; set; }
        public DateTime LastModifiedTimeUTC { get; set; }

        public string FileName { get; set; }
    }
}