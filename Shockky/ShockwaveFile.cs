using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shockky.IO;
using Shockky.Shockwave.Chunks;
using Shockky.Shockwave.Chunks.Container;
using Shockky.Shockwave.Chunks.Interface;

namespace Shockky
{
    public class ShockwaveFile : ChunkContainer, IDisposable
    {
        private readonly ShockwaveReader _input;

        public FileMetadataChunk Metadata { get; set; }
        
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
            : base()
        {
            _input = input;

            Metadata = new FileMetadataChunk(input);
        }
        public void Disassemble(Action<ChunkItem> callback = null)
        {
            //temporarily here
            
            if (Metadata.Codec == CodecKind.FGDM)
            {
                if (ReadChunk(true) is FileVersionChunk version)
                {
                    var fcdr = ReadChunk(true);
                    if (fcdr.Kind != ChunkKind.Fcdr) return;

                    if (ReadChunk(true) is AfterburnerMapChunk afterburnerMap)
                    {
                        var containerHeader = new CompressedChunkHeader(_input);
                        var container = new CompressedChunkContainer(_input, containerHeader);

                        container.ReadChunks(afterburnerMap, callback);
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
                    if (ReadChunk(_input, offset) is MemoryMapChunk mmapChunk)
                    {
                        ReadChunks(mmapChunk, callback);
                    }
                    else throw new Exception("what");
                }
            }
        }

        private ChunkItem ReadChunk(bool isCompressed = false)
        {
            var header = isCompressed ? new CompressedChunkHeader(_input) : new ChunkHeader(_input);
            return ReadChunk(_input, header);
        }
        private ChunkItem ReadChunk(ShockwaveReader input, int offset)
        {
            _input.Position = offset;

            var header = new ChunkHeader(input);
            var chunkInput = input.Cut(header.Length);

            return ReadChunk(chunkInput, header);
        }

        public override void ReadChunks(IChunkEntryMap entryMap, Action<ChunkItem> callback)
        {
            foreach (var entry in entryMap.Entries)
            {
                if (entry.Header.Kind == ChunkKind.free ||
                    entry.Header.Kind == ChunkKind.junk)
                {
                    Chunks.Add(new UnknownChunk(_input, entry.Header)); //TODO: eww
                    continue;
                }
                var chunk = ReadChunk(_input, entry.Offset);
                Chunks.Add(chunk);

                callback?.Invoke(chunk);
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
