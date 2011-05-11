using System.IO;
using System.Linq;
using OpenRasta.IO;
using OpenRasta.Web;

namespace Open.Documents
{
    public class DocumentDataHandler
    {
        public OperationResult Put(int id, byte[] content)
        {
            var document = Database.Documents.FirstOrDefault(x => x.Id == id);
            bool isNew = document.Data == null;

            document.Data = content;
            if (isNew)
                return new OperationResult.Created
                           {
                               RedirectLocation = document.DataHref
                           };
            return new OperationResult.OK();

        }
        public OperationResult Get(int id)
        {
            var doc = Database.Documents.FirstOrDefault(x => x.Id == id);
            var rawData = doc.Data;
            if (rawData == null)
                return new OperationResult.NotFound();
            return new OperationResult.OK
                       {
                           ResponseResource = new InMemoryFile(new MemoryStream(rawData))
                       {
                           ContentType = MediaType.ApplicationOctetStream,
                           FileName = doc.FileName
                       }
                       };
        }
    }
}