using Open.Documents;
using OpenDocuments.Interceptors;
using OpenRasta.Configuration.Fluent;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.Web;

namespace OpenRasta.Configuration
{
    public static class Configuration
    {
        public static void OpenDocuments(this IUses uses)
        {
            ResourceSpace.Uses
                .PipelineContributor<AddDocumentBookmarkContributor>();
            ResourceSpace.Has.Resource<DocumentInfo>()
                .Uri("/documents/{id}")
                .Handler<DocumentInfoHandler>()
                .XmlDataContract().And
                .JsonDataContract();

            ResourceSpace.Has.Resource<DocumentData>()
                .Uri("/documents/{id}/raw")
                .Handler<DocumentDataHandler>();

            ResourceSpace.Has.Resource<DocumentLibrary>()
                .Uri("/documents-somewhere")
                .Handler<DocumentLibraryHandler>()
                .XmlDataContract();
        }
    }
}