using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Shockky.IO;
using Shockky.Chunks;

namespace Shockky
{
    public class ShockwaveFile
    {
        private ReadOnlyMemory<byte> _input;

        public IList<ChunkItem> Chunks { get; }
        //public IDictionary<int, ChunkItem> Chunks { get; set; } //TODO: Might have to implement ChunkContainer abstraction to handle the resource/chunk identifiers and ownership etc.

        public DirectorVersion Version { get; set; }
        public FileMetadataChunk Metadata { get; set; }

        //TODO: Slow and fired too much anyways
        public ChunkItem this[int id]
            => Chunks.FirstOrDefault(chunk => chunk.Header.Id == id);

        public ShockwaveFile()
        {
            Chunks = new List<ChunkItem>();
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

        public void Disassemble(Action<ChunkItem> callback = default)
        {
            void HandleChunk(ChunkItem chunk)
            {
                callback?.Invoke(chunk);
                Chunks.Add(chunk);
            }

            var input = new ShockwaveReader(_input.Span, Metadata.IsBigEndian);
            input.Advance(Metadata.Header.GetBodySize() + Metadata.GetBodySize());

            if (Metadata.Codec == CodecKind.FGDM ||
                Metadata.Codec == CodecKind.FGDC)
            {
                throw new NotSupportedException("TODO: 'Spanified' decompression"); //plz "DeflateDecoder" 

                //if (ChunkItem.Read(input) is FileVersionChunk version &&
                //    ChunkItem.Read(input) is FileCompressionTypesChunk compressionTypes &&
                //    ChunkItem.Read(input) is AfterburnerMapChunk afterburnerMap &&
                //    ChunkItem.Read(input) is FGEIChunk fgei)
                //{
                //    var ilsChunk = fgei.ReadInitialLoadSegment(afterburnerMap.Entries[0]);
                //
                //    fgei.ReadChunks(afterburnerMap.Entries, HandleChunk);
                //    ilsChunk.ReadChunks(afterburnerMap.Entries, HandleChunk);
                //}
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
                            if (entry.Flags.HasFlag(ChunkEntryFlags.Ignore)) //TODO:
                            {
                                HandleChunk(new UnknownChunk(ref input, entry.Header));
                                continue;
                            }
                            input.AdvanceTo(entry.Offset);

                            ChunkItem chunk = ChunkItem.Read(ref input);
                            chunk.Header.Id = entry.Header.Id;
                            HandleChunk(chunk);
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
