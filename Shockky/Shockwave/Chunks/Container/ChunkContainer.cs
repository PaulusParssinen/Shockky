using System;
using System.Collections.Generic;
using System.Diagnostics;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Interface;

namespace Shockky.Shockwave.Chunks.Container
{
    public abstract class ChunkContainer : ShockwaveItem
    {
        public List<ChunkItem> Chunks { get; set; }

        protected ChunkContainer()
        {
            Chunks = new List<ChunkItem>();
        }

        public abstract void ReadChunks(IChunkEntryMap entryMap, Action<ChunkItem> callback);

        public static ChunkItem ReadChunk(ShockwaveReader input, ChunkHeader header)
        {
            switch (header.Kind)
            {
                case ChunkKind.Fver:
                    return new FileVersionChunk(input, header);
                case ChunkKind.Fcdr:
                    return new FileCompressionTypesChunk(input, header);
                case ChunkKind.ABMP:
                    return new AfterburnerMapChunk(input, header);
                case ChunkKind.ILS:
                    return new InitialLoadSegmentChunk(input, header);

                case ChunkKind.RIFX:
                    return new FileMetadataChunk(input, header);
                case ChunkKind.imap:
                    return new IndexMapChunk(input, header);
                case ChunkKind.mmap:
                    return new MemoryMapChunk(input, header);
                case ChunkKind.DRCF:
                    return new DRCFChunk(input, header);
                case ChunkKind.VWFI:
                    return new FileInfoChunk(input, header);
                case ChunkKind.KEYStar:
                    return new AssociationTableChunk(input, header);
                case ChunkKind.LctX:
                    return new ScriptContextChunk(input, header);
                case ChunkKind.Lscr:
                    return new ScriptChunk(input, header);
                case ChunkKind.Lnam:
                    return new NameTableChunk(input, header);
                case ChunkKind.CASStar:
                    return new CastAssociationTableChunk(input, header);
                //case ChunkKind.CASt:
                //return new CastMemberPropetiesChunk(input, header);
                case ChunkKind.Sord:
                    return new SortOrderChunk(input, header);
                case ChunkKind.CLUT:
                    return new PaletteChunk(input, header);
                case ChunkKind.MCsL:
                    return new MovieCastListChunk(input, header);
                case ChunkKind.STXT:
                    return new ScriptableTextChunk(input, header);
                case ChunkKind.FXmp:
                    return new FontMapChunk(input, header); 
                //case ChunkKind.XTRl:
                //    return new RequiredComponentLinkageChunk(input, header);
                default:
                    Debug.WriteLine("Unknown section occurred! Name: " + header.Name);
                    return new UnknownChunk(input, header);
            }
        }

        public override int GetBodySize()
        {
            throw new NotImplementedException();
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }
    }
}
