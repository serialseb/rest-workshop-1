using System;
using System.IO;
using OpenRasta.Web;

namespace Open.Documents
{
    public static class Database
    {
        static Database()
        {
            Documents = new DocumentLibrary
                            {
                                new DocumentInfo
                                    {
                                        Id = 1,
                                        Author = "@serialseb",
                                        Data = ReadNotepad(),
                                        DataHref = new DocumentData {Id = 1}.CreateUri(),
                                        LastModifiedTimeUTC = DateTime.UtcNow,
                                        FileName = "Notepad"
                                    }
                            };

        }

        private static byte[] ReadNotepad()
        {
            using (var stream = File.OpenRead(@"c:\windows\notepad.exe"))
            using (var reader = new BinaryReader(stream))
                return reader.ReadBytes((int) stream.Length);
        }

        public static DocumentLibrary Documents { get; set; }
    }
}