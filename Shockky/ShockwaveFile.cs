using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Shockky.IO;
using Shockky.Shockwave.Chunks;

namespace Shockky
{
    public class ShockwaveFile : IDisposable
    {
        private readonly ShockwaveReader _input;

        public List<ChunkItem> Chunks { get; }

        public DirectorVersion Version { get; set; }
        public FileMetadataChunk Metadata { get; set; }

        public ChunkItem this[int id]
            => Chunks?.FirstOrDefault(chunk => chunk.Header.Id == id);

        public ShockwaveFile()
        {
            Chunks = new List<ChunkItem>();
        }
        public ShockwaveFile(string path)
            : this(File.OpenRead(path))
        { }
        public ShockwaveFile(byte[] data)
            : this(new MemoryStream(data))
        { }
        public ShockwaveFile(Stream inputStream)
            : this(new ShockwaveReader(inputStream))
        { }

        public ShockwaveFile(ShockwaveReader input)
            : this()
        {
            _input = input;

            Metadata = new FileMetadataChunk(input);
        }

        public void Disassemble(Action<ChunkItem> callback = null)
        {
            if (Metadata.Codec == CodecKind.FGDM ||
                Metadata.Codec == CodecKind.FGDC)
            {
                if (ChunkItem.Read(_input) is FileVersionChunk version &&
                    ChunkItem.Read(_input) is FileCompressionTypesChunk fcdr &&
                    ChunkItem.Read(_input) is AfterburnerMapChunk afterburnerMap &&
                    ChunkItem.Read(_input) is FGEIChunk fgei)
                {
                    void HandleChunks(IEnumerable<ChunkItem> chunks)
                    {
                        foreach (var chunk in chunks)
                        {
                            callback?.Invoke(chunk);
                            Chunks.Add(chunk);
                        }
                    }

                    var ilsChunk = fgei.ReadInitialLoadSegment(afterburnerMap.Entries[0]);

                    HandleChunks(fgei.ReadChunks(afterburnerMap.Entries));
                    HandleChunks(ilsChunk.ReadChunks(afterburnerMap.Entries));
                }
            }
            else if (Metadata.Codec == CodecKind.MV93)
            {
                var imapChunk = ChunkItem.Read(_input) as InitialMapChunk;
                Version = imapChunk.Version;

                foreach (int offset in imapChunk.MemoryMapOffsets)
                {
                    _input.Position = offset;
                    if (ChunkItem.Read(_input) is MemoryMapChunk mmapChunk)
                    {
                        foreach (var entry in mmapChunk.Entries)
                        {
                            if (entry.Flags.HasFlag(ChunkEntryFlags.Ignore))
                            {
                                Chunks.Add(new UnknownChunk(_input, entry.Header)); //TODO:
                                continue;
                            }
                            _input.Position = entry.Offset;

                            var chunk = ChunkItem.Read(_input);
                            chunk.Header.Id = entry.Header.Id;

                            callback?.Invoke(chunk);
                            Chunks.Add(chunk);
                        }
                    }
                }
            }
        }

        public void Assemble()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _input.Dispose();
            }
        }
    }
}
