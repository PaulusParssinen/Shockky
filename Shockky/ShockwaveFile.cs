using System;
using System.IO;
using System.Collections.Generic;

using Shockky.IO;
using Shockky.Chunks;

namespace Shockky
{
    public class ShockwaveFile
    {
        private ReadOnlyMemory<byte> _input;

        //TODO: Some kind of abstraction, adding and modifying is a bit annoying like this. Also doing chunk lookups etc is weird now.
        public IDictionary<int, ChunkItem> Chunks { get; set; }

        public DirectorVersion Version { get; set; }
        public FileMetadataChunk Metadata { get; set; }

        public ChunkItem? this[int id]
        {
            get
            {
                Chunks.TryGetValue(id, out ChunkItem chunk);
                return chunk;
            }
        }

        public ShockwaveFile()
        {
            Chunks = new Dictionary<int, ChunkItem>();
        }
        public ShockwaveFile(string path)
            : this(File.ReadAllBytes(path))
        { }
        public ShockwaveFile(byte[] data)
            : this()
        {
            _input = data;

            var input = new ShockwaveReader(_input.Span);
            Metadata = new FileMetadataChunk(ref input);
        }

        public void Disassemble()
        {
            var input = new ShockwaveReader(_input.Span, Metadata.IsBigEndian);
            input.Advance(Metadata.Header.GetBodySize() + Metadata.GetBodySize());

            if (Metadata.Codec == CodecKind.FGDM ||
                Metadata.Codec == CodecKind.FGDC)
            {
                if (ChunkItem.Read(ref input) is FileVersionChunk version &&
                    ChunkItem.Read(ref input) is FileCompressionTypesChunk compressionTypes &&
                    ChunkItem.Read(ref input) is AfterburnerMapChunk afterburnerMap &&
                    ChunkItem.Read(ref input) is FileGzipEmbeddedImageChunk fgei)
                {
                    Chunks = fgei.ReadChunks(ref input, afterburnerMap.Entries);
                }
            }
            else if (Metadata.Codec == CodecKind.MV93)
            {
                var imapChunk = ChunkItem.Read(ref input) as InitialMapChunk;
                Version = imapChunk.Version;

                foreach (int offset in imapChunk.MemoryMapOffsets)
                {
                    input.AdvanceTo(offset);
                    if (ChunkItem.Read(ref input) is MemoryMapChunk mmapChunk)
                    {
                        foreach (ChunkEntry entry in mmapChunk.Entries)
                        {
                            if (entry.Flags.HasFlag(ChunkEntryFlags.Ignore))
                            {
                                Chunks.Add(entry.Id, new UnknownChunk(ref input, entry.Header));
                                continue;
                            }

                            input.AdvanceTo(entry.Offset);
                            Chunks.Add(entry.Id, ChunkItem.Read(ref input));
                        }
                    }
                }
            }

            //TODO:
            _input = null;
        }

        public void Assemble()
        {
            throw new NotImplementedException();
        }
    }
}
