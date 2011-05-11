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
                .OpenEverythingDoc()
                .And
                .JsonDataContract();

            ResourceSpace.Has.Resource<DocumentData>()
                .Uri("/documents/{id}/raw")
                .Handler<DocumentDataHandler>();

            ResourceSpace.Has.Resource<DocumentLibrary>()
                .Uri("/documents")
                .Handler<DocumentLibraryHandler>()
                .OpenEverythingDoc();
        }
        public static ICodecWithMediaTypeDefinition OpenEverythingDoc(this IHandlerForResourceWithUriDefinition root)
        {
            return root.XmlDataContract()
                .MediaType(new MediaType("application/vnd.openeverything.docs+xml;q=0.9"))
                .MediaType(new MediaType("application/xml"));
        }
    }
}