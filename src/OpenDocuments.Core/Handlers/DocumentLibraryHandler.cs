using System;
using System.IO;
using System.Linq;
using OpenRasta.IO;
using OpenRasta.Web;

namespace Open.Documents
{
    public class DocumentLibraryHandler
    {
        public DocumentLibrary Get()
        {
            return Database.Documents;
        }
        public OperationResult Post(DocumentInfo doc)
        {
            doc.Id = Database.Documents.Max(x => x.Id) + 1;
            
            doc.LastModifiedTimeUTC = DateTime.UtcNow;
            doc.DataHref = new DocumentData { Id = doc.Id }.CreateUri();
            var createdUri = doc.CreateUri();
            Database.Documents.Add(doc);

            return new OperationResult.Created
                       {
                           RedirectLocation = createdUri,
                           ResponseResource = doc
            };
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
}