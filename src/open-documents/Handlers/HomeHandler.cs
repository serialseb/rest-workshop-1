using System;
using Open.Documents.Resources;
using OpenRasta.Web;

namespace Open.Documents.Handlers
{
    public class Shoe
    {

    }

    public class HomeHandler
    {
        public Home Get()
        {
            return new Home { Title = "Welcome to OpenDocuments"};
        }
    }
}