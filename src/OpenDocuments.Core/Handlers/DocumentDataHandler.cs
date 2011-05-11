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
}