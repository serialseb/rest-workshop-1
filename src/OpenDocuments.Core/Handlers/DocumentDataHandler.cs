using System.IO;
using System.Linq;
using OpenRasta.IO;
using OpenRasta.Web;

namespace Open.Documents
{
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
}