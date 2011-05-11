using System;
using Open.Documents;
using OpenRasta.OperationModel.Interceptors;
using OpenRasta.Pipeline;
using OpenRasta.Web;

namespace OpenDocuments.Interceptors
{
    public class AddDocumentBookmarkContributor
        : IPipelineContributor
    {

        public AddDocumentBookmarkContributor()
        {
        }

        public void Initialize(IPipeline pipeline)
        {
            pipeline.Notify(WriteBookmark)
                .Before<KnownStages.IResponseCoding>();
        }

        private PipelineContinuation WriteBookmark(
            ICommunicationContext arg)
        {
            var uri = Database.Documents.CreateUri();
            arg.Response.Headers["Link"] += @", <" + uri + ">;" +
                                         "rel=\"http:/rels.openwrap.org/OpenDoc/index\"";
            return PipelineContinuation.Continue;
        }
    }
}