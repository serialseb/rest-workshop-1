using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Open.Documents.Resources;
using OpenRasta.Configuration;
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


            ResourceSpace.Uses.OpenDocuments();
            

        }
    }
}