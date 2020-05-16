using System;
using System.Linq;
using System.Collections.Generic;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class FileGzipEmbeddedImageChunk : ChunkItem
    {
        public FileGzipEmbeddedImageChunk()
            : base(ChunkKind.FGEI)
        { }
        public FileGzipEmbeddedImageChunk(ChunkHeader header)
            : base(header)
        { }

        //TODO: I'm gonna keep rewriting this. not happy with it at all
        public IDictionary<int, ChunkItem> ReadChunks(ref ShockwaveReader input, AfterBurnerMapEntry[] entries)
        {
            var chunks = new Dictionary<int, ChunkItem>(entries.Length);

            ReadInitialLoadSegment(ref input, entries, chunks);

            for (int i = 1; i < entries.Length; i++)
            {
                AfterBurnerMapEntry entry = entries[i];

                if (entry.Offset == -1) continue;

                input.AdvanceTo(Header.Offset + entry.Offset);

                if (entry.IsCompressed)
                {
                    //TODO: Clean-up the decompression code. Polluted this section too much. 
                    ReadOnlySpan<byte> compressedData = input.ReadBytes(entry.CompressedLength);

                    Span<byte> decompressedData = entry.DecompressedLength <= 1024 ?
                        stackalloc byte[entry.DecompressedLength] : new byte[entry.DecompressedLength];

                    ZLib.Decompress(compressedData, decompressedData);

                    ShockwaveReader chunkInput = new ShockwaveReader(decompressedData, input.IsBigEndian); //TODO: Check whether the endianness is preserved in the compressed chunks

                    chunks.Add(entry.Id, Read(ref chunkInput, entry.Header));
                }
                else chunks.Add(entry.Id, Read(ref input, entry.Header));                
            }
            return chunks;
        }

        private void ReadInitialLoadSegment(ref ShockwaveReader input, AfterBurnerMapEntry[] entries, Dictionary<int, ChunkItem> chunks)
        {
            //First entry in the AfterBurnerMap must be ILS.
            AfterBurnerMapEntry ilsEntry = entries[0];
            input.AdvanceTo(Header.Offset + ilsEntry.Offset);

            //TODO: AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAArgh
            ReadOnlySpan<byte> compressedData = input.ReadBytes(ilsEntry.CompressedLength);

            Span<byte> decompressedData = ilsEntry.DecompressedLength <= 1024 ?
                    stackalloc byte[ilsEntry.DecompressedLength] : new byte[ilsEntry.DecompressedLength]; //TODO: yea that's never gonne be under stack limit lmao

            ZLib.Decompress(compressedData, decompressedData);

            ShockwaveReader ilsReader = new ShockwaveReader(decompressedData, input.IsBigEndian);

            while (ilsReader.IsDataAvailable)
            {
                int id = ilsReader.Read7BitEncodedInt();

                AfterBurnerMapEntry entry = entries.FirstOrDefault(e => e.Id == id);
                if (entry == null) break;

                entry.Header.Offset = ilsReader.Position;
                chunks.Add(id, Read(ref ilsReader, entry.Header));
            }
        }


        public override int GetBodySize()
        {
            throw new NotImplementedException();
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }
    }
}
