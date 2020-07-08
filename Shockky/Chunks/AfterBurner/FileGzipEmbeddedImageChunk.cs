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

        //TODO: Tidy up more.
        public IDictionary<int, ChunkItem> ReadChunks(ref ShockwaveReader input, AfterBurnerMapEntry[] entries)
        {
            int chunkStart = input.Position;
            var chunks = new Dictionary<int, ChunkItem>(entries.Length);

            ReadInitialLoadSegment(ref input, entries, chunks);

            for (int i = 1; i < entries.Length; i++)
            {
                AfterBurnerMapEntry entry = entries[i];
                if (entry.Offset == -1) continue;

                input.Position = chunkStart + entry.Offset;
                chunks.Add(entry.Id, Read(ref input, entry));
            }
            return chunks;
        }

        private void ReadInitialLoadSegment(ref ShockwaveReader input, AfterBurnerMapEntry[] entries, Dictionary<int, ChunkItem> chunks)
        {
            //First entry in the AfterBurnerMap must be ILS.
            AfterBurnerMapEntry ilsEntry = entries[0];
            input.Advance(ilsEntry.Offset);

            //TODO: this shouldn't be here
            ReadOnlySpan<byte> compressedData = input.ReadBytes(ilsEntry.Length);

            Span<byte> decompressedData = ilsEntry.DecompressedLength <= 1024 ?
                    stackalloc byte[ilsEntry.DecompressedLength] : new byte[ilsEntry.DecompressedLength];

            ZLib.Decompress(compressedData, decompressedData);

            ShockwaveReader ilsReader = new ShockwaveReader(decompressedData, input.IsBigEndian);

            while (ilsReader.IsDataAvailable)
            {
                int id = ilsReader.ReadVarInt();

                AfterBurnerMapEntry entry = entries.FirstOrDefault(e => e.Id == id); //TODO: Chunk entries as dictionary
                if (entry == null) break;

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
