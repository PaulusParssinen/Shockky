using System.Diagnostics;
using System.Collections.Generic;

using Shockky.IO;

namespace Shockky.Chunks
{
    [DebuggerDisplay("{Kind}")]
    public abstract class ChunkItem : ShockwaveItem
    {
        public ChunkKind Kind => Header.Kind;
        public ChunkHeader Header { get; set; }

        public Queue<object> Remnants { get; set; }

        protected ChunkItem(ChunkKind kind)
            : this(new ChunkHeader(kind))
        { }
        protected ChunkItem(ShockwaveReader input)
            : this(new ChunkHeader(input))
        { }
        protected ChunkItem(ChunkHeader header)
        {
            Header = header;

            Remnants = new Queue<object>();
        }

        public ShockwaveReader WrapDecompressor(in ShockwaveReader input)
        {
            long dataLeft = Header.Length - (input.Position - Header.Offset) - 2; // Include ZLib header
            return input.WrapDecompressor((int)dataLeft);
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            Header.Length = GetBodySize();
            output.Write(Header);
            WriteBodyTo(output);
        }

        public abstract void WriteBodyTo(ShockwaveWriter output);

        public static ChunkItem Read(ShockwaveReader input)
        {
            return Read(input, new ChunkHeader(input));
        }
        public static ChunkItem Read(ShockwaveReader input, ChunkHeader header)
        {
            switch (header.Kind)
            {
                case ChunkKind.Fver:
                    return new FileVersionChunk(input, header);
                case ChunkKind.Fcdr:
                    return new FileCompressionTypesChunk(input, header);
                case ChunkKind.ABMP:
                    return new AfterburnerMapChunk(input, header);
                case ChunkKind.FGEI:
                    return new FGEIChunk(input, header);
                case ChunkKind.ILS:
                    return new InitialLoadSegmentChunk(input, header);
                    
                case ChunkKind.RIFX:
                    return new FileMetadataChunk(input, header);
                case ChunkKind.imap:
                    return new InitialMapChunk(input, header);
                case ChunkKind.mmap:
                    return new MemoryMapChunk(input, header);
                case ChunkKind.KEYStar:
                    return new AssociationTableChunk(input, header);

                case ChunkKind.VWCF:
                case ChunkKind.DRCF:
                    return new ConfigChunk(input, header);

                //case ChunkKind.VWSC:
                //    return new VWScoreChunk(input, header);
                case ChunkKind.VWLB:
                    return new WVLabelChunk(input, header);
                case ChunkKind.VWFI:
                    return new FileInfoChunk(input, header);

                case ChunkKind.LctX:
                    return new ScriptContextChunk(input, header);
                case ChunkKind.Lscr:
                    return new ScriptChunk(input, header);
                case ChunkKind.Lnam:
                    return new NameTableChunk(input, header);

                case ChunkKind.CASStar:
                    return new CastAssociationTableChunk(input, header);
                case ChunkKind.CASt:
                    return new CastMemberPropertiesChunk(input, header);
                    
                //case ChunkKind.Cinf:
                //    return new CastInfoChunk(input, header);
                //case ChunkKind.SCRF:
                //    return new ScoreReferenceChunk(input, header);
                case ChunkKind.Sord:
                    return new SortOrderChunk(input, header);
                case ChunkKind.CLUT:
                    return new PaletteChunk(input, header);
                case ChunkKind.MCsL:
                    return new MovieCastListChunk(input, header);
                case ChunkKind.STXT:
                    return new StyledTextChunk(input, header);

                //case ChunkKind.Fmap:
                //    return new CastFontMapChunk(input, header);
                case ChunkKind.FXmp:
                    return new FontMapChunk(input, header);
                    
                case ChunkKind.XTRl:
                    return new XtraListChunk(input, header);

                //case ChunkKind.PUBL:
                //    return new PublishSettingsChunk(input, header);
                //case ChunkKind.GRID:
                //    return new GridChunk(input, header);

                case ChunkKind.snd:
                    return new SoundDataChunk(input, header);
                case ChunkKind.BITD:
                    return new BitmapChunk(input, header);

                default:
                    Debug.WriteLine("Unknown section | " + header.Kind.ToFourCC());
                    return new UnknownChunk(input, header);
            }
        }
    }
}
