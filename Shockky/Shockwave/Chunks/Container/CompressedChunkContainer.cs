using System;
using System.Collections.Generic;
using System.Diagnostics;
using Shockky.Compression;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Container;
using Shockky.Shockwave.Chunks.Interface;

namespace Shockky.Shockwave.Chunks
{
    //TODO: Naming of this thing
    public class CompressedChunkContainer : ChunkContainer
    {
        private readonly ShockwaveReader _input;
        
        public CompressedChunkContainer(ShockwaveReader input, ChunkHeader header)
            : base()
        {
            _input = input.Cut(header.Length);
        }

        public override void ReadChunks(IChunkEntryMap entryMap, Action<ChunkItem> callback)
        {
            InitialLoadSegmentChunk ilsChunk = null;
            var ilsEntries = new Dictionary<int, IChunkEntry>();

            foreach(var entry in entryMap.Entries)
            {
                if (entry.Offset == -1) ilsEntries.Add(entry.Id, entry);
                else if (entry.IsCompressed)
                {
                    _input.Position = entry.Offset;
                    var anotherinputcoswhythefucknot = _input.Cut(entry.Header.Length);
                    var chunkInput = ZLIB.WrapDecompressor(anotherinputcoswhythefucknot.BaseStream);
                    var chunk = ReadChunk(chunkInput, entry.Header);

                    if (chunk.Kind == ChunkKind.ILS)
                        ilsChunk = chunk as InitialLoadSegmentChunk;

                    callback?.Invoke(chunk);

                    Chunks.Add(chunk);
                }
                else
                {
                    throw new NotImplementedException("Gotta find an example file having this shit");
                    //TODO: Could be just this

                    _input.Position = entry.Offset;
                    var chunkInput = _input.Cut(entry.Header.Length);
                    var chunk = ReadChunk(chunkInput, entry.Header);

                    Chunks.Add(chunk);
                }
            }

            if (ilsChunk == null) throw new Exception("Oh for fucks sake");

            if (!ilsChunk.TryReadChunks(ilsEntries, out var ilsChunks))
                throw new NotImplementedException("Structure gets fucked up if theres not fully implemented chunk in the Initial Load Segment.");
            Chunks.AddRange(ilsChunks);
        }
        
        public override int GetBodySize()
        {
            throw new NotImplementedException();
        }
    }
}
