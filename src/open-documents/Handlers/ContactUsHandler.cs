using Open.Documents.Resources;

namespace Open.Documents.Handlers
{
    public class ContactUsHandler
    {
        public ContactUs Post(string email, string comment)
        {
            return Get(email, comment);
        }
        public ContactUs Get(string email, string comment)
        {
            return new ContactUs
                       {
                           Email = email,
                           Comment = comment
                       };
        }
    }
}