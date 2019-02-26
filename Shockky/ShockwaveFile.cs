using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Shockky.IO;
using Shockky.Chunks;

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
            void HandleChunk(ChunkItem chunk)
            {
                callback?.Invoke(chunk);
                Chunks.Add(chunk);
            }

            if (Metadata.Codec == CodecKind.FGDM ||
                Metadata.Codec == CodecKind.FGDC)
            {
                if (ChunkItem.Read(_input) is FileVersionChunk version &&
                    ChunkItem.Read(_input) is FileCompressionTypesChunk fcdr &&
                    ChunkItem.Read(_input) is AfterburnerMapChunk afterburnerMap &&
                    ChunkItem.Read(_input) is FGEIChunk fgei)
                {
                    var ilsChunk = fgei.ReadInitialLoadSegment(afterburnerMap.Entries[0]);

                    fgei.ReadChunks(afterburnerMap.Entries, HandleChunk);
                    ilsChunk.ReadChunks(afterburnerMap.Entries, HandleChunk);
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
                        foreach (ChunkEntry entry in mmapChunk.Entries)
                        {
                            if (entry.Flags.HasFlag(ChunkEntryFlags.Ignore))
                            {
                                HandleChunk(new UnknownChunk(_input, entry.Header)); //TODO:
                                continue;
                            }
                            _input.Position = entry.Offset;

                            ChunkItem chunk = ChunkItem.Read(_input);
                            chunk.Header.Id = entry.Header.Id;
                            HandleChunk(chunk);
                        }
                    }
                }
            }
        }

        public void Assemble(ShockwaveWriter output)
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
