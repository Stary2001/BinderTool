using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BinderTool.Core.Bnd4
{
    public class Bnd3File
    {
        private const string Bnd3Signature = "BND3";
        private readonly List<Bnd3FileEntry> _entries;

        public Bnd3File()
        {
            _entries = new List<Bnd3FileEntry>();
        }

        public List<Bnd3FileEntry> Entries => _entries;

        public static Bnd3File ReadBnd3File(Stream inputStream)
        {
            Bnd3File bnd3File = new Bnd3File();
            bnd3File.Read(inputStream);
            return bnd3File;
        }

        private void Read(Stream inputStream)
        {
            BinaryReader reader = new BinaryReader(inputStream, Encoding.GetEncoding("Shift-JIS"), true);
            string signature = reader.ReadString(4);
            if (signature != Bnd3Signature)
                throw new Exception("Unknown signature");
            string id = reader.ReadString(8);
            int version = reader.ReadInt32();
            if (version != 0x74 && version != 0x54 && version != 0x5c && version != 0x7c && version != 0x78)
                throw new InvalidDataException();

            int recordCount = reader.ReadInt32();
            int totalHeaderSize = reader.ReadInt32(); // Either zero or the unaligned end of the last record's name before the first record's data.
            reader.Skip(8);
            
            if(version == 0x5c)
            {

            }
            long p = reader.GetPosition();
            for (int i = 0; i < recordCount; i++)
            {
                reader.Skip(4);
                int fileSize = reader.ReadInt32();
                int fileOffset = reader.ReadInt32();
                int fileId = reader.ReadInt32();
                int hmm = reader.ReadInt32();
                int fileNameOffset = reader.ReadInt32();
                if (version != 0x78)
                {
                    int fileSize2 = reader.ReadInt32();
                }
                
                long offset = reader.GetPosition();

                string fileName = "";
                if (fileNameOffset > 0)
                {
                    reader.Seek(fileNameOffset);
                    fileName = reader.ReadNullTerminatedString();
                }
                
                reader.Seek(fileOffset);
                _entries.Add(Bnd3FileEntry.Read(inputStream, fileSize, fileName));
                reader.Seek(offset);

            }
        }
    }
}
