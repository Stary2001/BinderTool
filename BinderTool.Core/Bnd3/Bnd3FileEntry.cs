using System.IO;
using System.Text;

namespace BinderTool.Core.Bnd4
{
    public class Bnd3FileEntry
    {
        public byte[] EntryData { get; private set; }
        public string FileName { get; private set; }

        public static Bnd3FileEntry Read(Stream inputStream, int fileSize, string fileName)
        {
            Bnd3FileEntry result = new Bnd3FileEntry();
            BinaryReader reader = new BinaryReader(inputStream, Encoding.UTF8, true);
            result.FileName = fileName;
            result.EntryData = reader.ReadBytes(fileSize);
            return result;
        }
    }
}
