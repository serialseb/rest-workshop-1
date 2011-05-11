using System.Linq;

namespace Open.Documents
{
    public class DocumentInfoHandler
    {
        public DocumentInfo Get(int id)
        {
            return Database.Documents.FirstOrDefault(x => x.Id == id);
        }
    }
}