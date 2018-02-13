using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Shockky.IO;
using Shockky.Shockwave.Chunks;
using Shockky.Shockwave.Chunks.Enum;

namespace Shockky
{
    public class ShockwaveFile : IDisposable
    {
        private readonly ShockwaveReader _reader;

        public List<ChunkItem> Chunks { get; set; }

        public bool IsLittleEndian { get; set; }
        public int FileLength { get; set; }
        public string Codec { get; set; }

        public ShockwaveFile(string path)
            : this(File.OpenRead(path))
        { }
        public ShockwaveFile(byte[] data)
            : this(new MemoryStream(data))
        { }
        public ShockwaveFile(Stream inputStream)
            : this(new ShockwaveReader(inputStream))
        { }

        public ShockwaveFile(ShockwaveReader reader)
        {
            _reader = reader;

            IsLittleEndian = (_reader.ReadReversedString(4) == "RIFX");

            if (!IsLittleEndian)
                throw new NotImplementedException("Yo nigga wait");

            FileLength = _reader.ReadInt32();

            Codec = _reader.ReadReversedString(4); //FGDM || MV93
        }

        public void Disassemble()
        {
            Disassemble(null);
        }
        public void Disassemble(Action<ChunkItem, MemoryMapChunk> callback)
        {
            if(!(ReadChunk(_reader) is IndexMapChunk iMapChunk))
                throw new InvalidCastException("I did not see this coming..");

            foreach (int offset in iMapChunk.MemoryMapOffsets)
            {
                _reader.Position = offset;

                if (ReadChunk(_reader) is MemoryMapChunk mmapChunk)
                {
                    Chunks = new List<ChunkItem>(mmapChunk.ChunksUsed);

                    foreach (var entry in mmapChunk.ChunkEntries)
                    {
                        var chunk = ReadChunk(entry, _reader);
                        Chunks.Add(chunk);

                        callback?.Invoke(chunk, mmapChunk);
                    }
                }
                else throw new Exception("What the actual fuck, tho this could be obfuscation troll in \"future\"");
            }
        }
        public ChunkItem ReadChunk(ShockwaveReader input)
        {
            return ReadChunk(new ChunkEntry(input, -1, false), input); //TODO: -1? Rethink this
        }
        public ChunkItem ReadChunk(ChunkEntry entry, ShockwaveReader input)
        {
            if (entry.Offset > 0)
                _reader.Position = entry.Offset + 8; //We already read the header, skip that

	        var chunkInput = _reader.Cut(entry.Header.Length);

            switch (entry.Header.Type)
            {
                case ChunkType.imap:
                    return new IndexMapChunk(chunkInput, entry);
                case ChunkType.mmap:
                    return new MemoryMapChunk(chunkInput, entry);
                case ChunkType.DRCF:
                    return new DRCFChunk(chunkInput, entry);
                case ChunkType.VWFI:
                    return new FileInfoChunk(chunkInput, entry);
                case ChunkType.KEYStar:
                    return new AssociationTableChunk(chunkInput, entry);
             //   case ChunkType.CASStar: return null;
                    //return new CastAssociationTableChunk(chunkInput, entry);
                case ChunkType.LctX:
                    return new ScriptContextChunk(chunkInput, entry);
                case ChunkType.Lscr:
                    return new ScriptChunk(chunkInput, entry);
                case ChunkType.Lnam:
                    return new NameTableChunk(chunkInput, entry);
                //case ChunkType.CASt:
                  //  return null;//new CastChunk(chunkInput, entry);
                case ChunkType.CLUT:
                    return new PaletteChunk(chunkInput, entry);
                case ChunkType.MCsL:
                    return new MovieCastListChunk(chunkInput, entry);
                case ChunkType.STXT:
                    return new ScriptableTextChunk(chunkInput, entry);
                default:
                    Debug.WriteLine("Unknown section occurred! Name: " + entry.Header.Name);
                    return new UnknownChunk(entry.Header, chunkInput);
            }
        }

        public void Assemble()
        {
            //TODO: Adjust stuff here etc
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _reader.Dispose();
            }
        }
    }
}
