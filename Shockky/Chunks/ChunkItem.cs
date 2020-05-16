using System.Diagnostics;
using System.Collections.Generic;

using Shockky.IO;

namespace Shockky.Chunks
{
    [DebuggerDisplay("[{Kind}] Length: {Header.Length}")]
    public abstract class ChunkItem : ShockwaveItem
    {
        public ChunkHeader Header { get; set; }
        public Queue<object> Remnants { get; set; } //

        public ChunkKind Kind => Header.Kind;

        protected ChunkItem(ChunkKind kind)
            : this(new ChunkHeader(kind))
        { }
        protected ChunkItem(ChunkHeader header)
        {
            Header = header;

            Remnants = new Queue<object>();
        }

        public DeflateShockwaveReader CreateDeflateReader(ref ShockwaveReader input)
        {
            input.ReadBytes(2); //Skip ZLib header

            int dataLeft = Header.Length - (input.Position - Header.Offset);
            byte[] compressedData = input.ReadBytes(dataLeft).ToArray();

            return new DeflateShockwaveReader(compressedData, input.IsBigEndian);
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            Header.Length = GetBodySize();
            Header.WriteTo(output);

            WriteBodyTo(output);
        }
        public abstract void WriteBodyTo(ShockwaveWriter output);

        public static ChunkItem Read(ref ShockwaveReader input)
        {
            return Read(ref input, new ChunkHeader(ref input));
        }
        public static ChunkItem Read(ref ShockwaveReader input, ChunkHeader header)
        {
            return header.Kind switch
            {
                ChunkKind.Fver => new FileVersionChunk(ref input, header),
                ChunkKind.Fcdr => new FileCompressionTypesChunk(ref input, header),
                ChunkKind.ABMP => new AfterburnerMapChunk(ref input, header),
                ChunkKind.FGEI => new FileGzipEmbeddedImageChunk(header),
                
                ChunkKind.RIFX => new FileMetadataChunk(ref input, header),
                ChunkKind.imap => new InitialMapChunk(ref input, header),
                ChunkKind.mmap => new MemoryMapChunk(ref input, header),
                ChunkKind.KEYPtr => new AssociationTableChunk(ref input, header),

                ChunkKind.VWCF => new ConfigChunk(ref input, header),
                ChunkKind.DRCF => new ConfigChunk(ref input, header),

                //ChunkKind.VWSC => new ScoreChunk(ref input, header),
                ChunkKind.VWLB => new ScoreLabelChunk(ref input, header),
                ChunkKind.VWFI => new FileInfoChunk(ref input, header),

                ChunkKind.LctX => new ScriptContextChunk(ref input, header),
                ChunkKind.Lscr => new ScriptChunk(ref input, header),
                ChunkKind.Lnam => new NameTableChunk(ref input, header),

                ChunkKind.CASPtr => new CastAssociationTableChunk(ref input, header),
                ChunkKind.CASt => new CastMemberPropertiesChunk(ref input, header),

                ChunkKind.MCsL => new MovieCastListChunk(ref input, header),

                //ChunkKind.Cinf => new CastInfoChunk(ref input, header),
                ChunkKind.SCRF => new ScoreReferenceChunk(ref input, header),
                ChunkKind.Sord => new SortOrderChunk(ref input, header),
                ChunkKind.CLUT => new PaletteChunk(ref input, header),
                ChunkKind.STXT => new StyledTextChunk(ref input, header),

                //case ChunkKind.Fmap => new CastFontMapChunk(ref input, header),

                ChunkKind.XTRl => new XtraListChunk(ref input, header),

                //case ChunkKind.PUBL => new PublishSettingsChunk(ref input, header),
                //case ChunkKind.GRID => new GridChunk(ref input, header),

                ChunkKind.FXmp => new FontMapChunk(ref input, header),
                ChunkKind.snd => new SoundDataChunk(ref input, header),
                ChunkKind.BITD => new BitmapChunk(ref input, header),

                _ => new UnknownChunk(ref input, header),
            };
        }
    }
}
