using System;
using System.Runtime.Serialization;

namespace Open.Documents
{
    public class DocumentInfo
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public Uri DataHref { get; set; }
        [IgnoreDataMember]
        public byte[] Data { get; set; }
        public DateTime LastModifiedTimeUTC { get; set; }

        public string FileName { get; set; }
    }
}