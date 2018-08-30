using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Shockky.IO;
using Shockky.Shockwave.Chunks;

namespace Shockky
{
    public class ShockwaveFile : IDisposable
    {
        private readonly ShockwaveReader _input;

        public List<ChunkItem> Chunks { get; set; }

        public FileMetadataChunk Metadata { get; set; }

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
            if (Metadata.Codec == CodecKind.FGDM)
            {
                if (ReadChunk() is FileVersionChunk version)
                {
                    if (ReadChunk() is FileCompressionTypesChunk fcdr)
                    {
                        if (ReadChunk() is AfterburnerMapChunk afterburnerMap)
                        {
                            if (ReadChunk() is FGEIChunk fgei)
                            {
                                Debug.Assert(afterburnerMap.Entries[0].Header.Kind == ChunkKind.ILS, "HM");
                                
                                Chunks = new List<ChunkItem>(afterburnerMap.Entries.Count);

                                var ilsChunk = fgei.ReadInitialLoadSegment(afterburnerMap.Entries[0]);
                                var chunks = fgei.ReadChunks(afterburnerMap.Entries);

                                if (!ilsChunk.TryReadChunks(afterburnerMap.Entries, out var ilsChunks))
                                    throw new NotImplementedException("Chunk reading failed");
                                
                                Chunks.AddRange(ilsChunks);
                                Chunks.AddRange(chunks);
                            }
                        }
                    }
                }
            }
            else if (Metadata.Codec == CodecKind.MV93)
            {
                var imapChunk = ReadChunk() as IndexMapChunk;

                if (imapChunk == null)
                    throw new InvalidCastException("I did not see this coming..");

                foreach (int offset in imapChunk.MemoryMapOffsets)
                {
                    _input.Position = offset;
                    if (ReadChunk() is MemoryMapChunk mmapChunk)
                    {
                        foreach (var entry in mmapChunk.Entries)
                        {
                            if (entry.Header.Kind == ChunkKind.free ||
                                entry.Header.Kind == ChunkKind.junk)
                            {
                                Chunks.Add(new UnknownChunk(_input, entry.Header)); //TODO: eww
                                continue;
                            }
                            _input.Position = entry.Offset;
                            var chunk = ReadChunk();

                            callback?.Invoke(chunk);
                            Chunks.Add(chunk);
                        }
                    }
                    else throw new Exception("what");
                }
            }
        }

        private ChunkItem ReadChunk()
        {
            return ChunkItem.Read(_input, new ChunkHeader(_input));
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
