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
        private ShockwaveReader _input;

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

        public ShockwaveFile(ShockwaveReader input) //TODO: Validate if it even is a Shockwave movie/cast whatever shit projector
        {
            IsLittleEndian = (input.ReadString(4, true) == "RIFX"); //Magic header

            if (!IsLittleEndian)
                throw new NotImplementedException("Yo nigga hol' up");

            FileLength = input.ReadInt32();

            Codec = input.ReadString(4, true); //FGDM || MV93

            _input = input;
        }

        public void Dissassemble()
        {
            if(!(ReadChunk() is IndexMapChunk iMapChunk))
                throw new InvalidCastException("I didn't see this coming..");

            foreach (int offset in iMapChunk.MemoryMapOffsets)
            {
                _input.Position = offset;

                if(!(ReadChunk() is MemoryMapChunk mmapChunk))
                    throw new Exception("tf");

                Chunks = new List<ChunkItem>(mmapChunk.ChunksUsed);

                foreach (var entry in mmapChunk.ChunkEntries.Where(e => e.Header.Type != ChunkType.free))
                {
                    Console.WriteLine($"[{mmapChunk.ChunkEntries.IndexOf(entry)}] Name: {entry.Header.Name}, Offset: {entry.Offset}, Length: {entry.Header.Length}");

                   // if(entry.Header.Type == ChunkType.mmap) continue;

                    var chunk = ReadChunk(entry);
                    Chunks.Add(chunk);

                    //Area51 (Sandbox/Debugging)
                    switch (chunk.Header.Type)
                    {
                        case ChunkType.CASStar:
                        {
                            var castAssocChunk = chunk as CastAssociationTableChunk;
                            for (int i = 1; i < castAssocChunk.Members.Count; i++)
                            {
                                var castMemberEntry = mmapChunk.ChunkEntries[castAssocChunk.Members[i]];
                                var castMember = ReadChunk(castMemberEntry) as CastChunk;
                            }
                        }
                        break;
                        case ChunkType.LctX:
                        {
                            var contextChunk = chunk as ScriptContextChunk;

                            var nameListChunk =
                                ReadChunk(mmapChunk.ChunkEntries[contextChunk.NameTableSectionId])  as NameTableChunk;

                            if(nameListChunk == null)
                                throw new Exception("Invalid NameTable chunk ID given in ScriptContextChunk!");

                            foreach (var section in contextChunk.Sections) //so here we loop 'em
                            {
                                if(section.SectionId == -1) continue; //GAY yeah idk why this even exist, prob means that they have deleted something from the movie.. idk

                                //var testChunkEntry = mmapChunk.ChunkEntries[section.Link];
                                var scriptChunkEntry = mmapChunk.ChunkEntries[section.SectionId];//Get the scriptchunk which the section refers to
                                _input.Position = scriptChunkEntry.Offset + 8; //string 4 length = chunk name, int32 length //skippy da header ye | we currently are at the position where the script chunk begins!

                                var chunkInput = new ShockwaveReader(_input, scriptChunkEntry.Header.Length);//Let's cut the reader again inorder to align the offset for the chunk, chunks usually use the starting position of the chunk in those..

                                var scriptChunk = new ScriptChunk(chunkInput, entry, nameListChunk.Names); //Pass the new reader, entry, and the list of names to it.
                            }
                        }
                        break;
                        case ChunkType.Lnam:
                        {
                            var nameChunk = chunk as NameTableChunk;

                            Console.Write("Name Table: ");
                            foreach (var name in nameChunk.Names.OrderBy(n => n))
                            {
                                Console.Write(name +  ", ");
                            }
                        }
                        break;
                    }
                }
            }
        }

        public ChunkItem ReadChunk()
        {
            return ReadChunk(new ChunkEntry(ref _input, false));
        }
        public ChunkItem ReadChunk(ChunkEntry entry)
        {
            if (entry.Offset > 0)
                _input.Position = entry.Offset + 8; //We readed the header already, skip that

            var chunkInput = new ShockwaveReader(_input, entry.Header.Length); //In the future when we read these chunks

            switch (entry.Header.Type) //TODO: I can do better
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
                case ChunkType.CASStar:
                    return new CastAssociationTableChunk(chunkInput, entry);
                case ChunkType.LctX:
                    return new ScriptContextChunk(chunkInput, entry);
                case ChunkType.Lnam:
                    return new NameTableChunk(chunkInput, entry);
                case ChunkType.CASt:
                    return new CastChunk(chunkInput, entry);
                case ChunkType.CLUT:
                    return new PaletteChunk(chunkInput, entry);
                case ChunkType.MCsL:
                    return new MovieCastListChunk(chunkInput, entry);
                case ChunkType.STXT:
                    return new ScriptableTextChunk(chunkInput, entry);
                default:
                    Debug.WriteLine("Unknown section! Name: " + entry.Header.Name);
                    return new ChunkItem(entry.Header);
            }
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
