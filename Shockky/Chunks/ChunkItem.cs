using System.Diagnostics;
using System.Collections.Generic;

using Shockky.IO;

namespace Shockky.Chunks
{
    [DebuggerDisplay("[{Header.Id}] {Kind}")]
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
            return header.Kind switch
            {
                ChunkKind.Fver => new FileVersionChunk(input, header),
                ChunkKind.Fcdr => new FileCompressionTypesChunk(input, header),
                ChunkKind.ABMP => new AfterburnerMapChunk(input, header),
                ChunkKind.FGEI => new FGEIChunk(input, header),
                ChunkKind.ILS => new InitialLoadSegmentChunk(input, header),

                ChunkKind.RIFX => new FileMetadataChunk(input, header),
                ChunkKind.imap => new InitialMapChunk(input, header),
                ChunkKind.mmap => new MemoryMapChunk(input, header),
                ChunkKind.KEYStar => new AssociationTableChunk(input, header),

                ChunkKind.VWCF => new ConfigChunk(input, header),
                ChunkKind.DRCF => new ConfigChunk(input, header),

                //ChunkKind.VWSC => new VWScoreChunk(input, header),
                ChunkKind.VWLB => new WVLabelChunk(input, header),
                ChunkKind.VWFI => new FileInfoChunk(input, header),

                ChunkKind.LctX => new ScriptContextChunk(input, header),
                ChunkKind.Lscr => new ScriptChunk(input, header),
                ChunkKind.Lnam => new NameTableChunk(input, header),

                ChunkKind.CASStar => new CastAssociationTableChunk(input, header),
                ChunkKind.CASt => new CastMemberPropertiesChunk(input, header),

                ChunkKind.MCsL => new MovieCastListChunk(input, header),

                //ChunkKind.Cinf => new CastInfoChunk(input, header),
                ChunkKind.SCRF => new ScoreReferenceChunk(input, header),
                ChunkKind.Sord => new SortOrderChunk(input, header),
                ChunkKind.CLUT => new PaletteChunk(input, header),
                ChunkKind.STXT => new StyledTextChunk(input, header),

                //case ChunkKind.Fmap => new CastFontMapChunk(input, header),
                ChunkKind.FXmp => new FontMapChunk(input, header),

                ChunkKind.XTRl => new XtraListChunk(input, header),

                //case ChunkKind.PUBL => new PublishSettingsChunk(input, header),
                //case ChunkKind.GRID => new GridChunk(input, header),

                ChunkKind.snd => new SoundDataChunk(input, header),
                ChunkKind.BITD => new BitmapChunk(input, header),

                _ => new UnknownChunk(input, header),
            };
        }
    }
}
