using System;
using System.Diagnostics;
using System.Collections.Generic;

using Shockky.IO;

namespace Shockky.Chunks
{
    [DebuggerDisplay("[{Kind}] Length: {Header.Length}")]
    public abstract class ChunkItem : ShockwaveItem
    {
        public ChunkHeader Header { get; set; }
        public Queue<object> Remnants { get; set; }

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
            input.Advance(2); //Skip ZLib header

            int dataLeft = Header.Length - input.Position;
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
        public static ChunkItem Read(ref ShockwaveReader input, AfterBurnerMapEntry entry)
        {
            return entry.IsCompressed ? input.ReadCompressedChunk(entry) : 
                Read(ref input, entry.Header);
        }
        public static ChunkItem Read(ref ShockwaveReader input, ChunkHeader header)
        {
            ReadOnlySpan<byte> chunkSpan = input.ReadBytes(header.Length);
            ShockwaveReader chunkInput = new ShockwaveReader(chunkSpan, input.IsBigEndian);

            return header.Kind switch
            {
                ChunkKind.RIFX => new FileMetadataChunk(ref chunkInput, header),
                
                ChunkKind.Fver => new FileVersionChunk(ref chunkInput, header),
                ChunkKind.Fcdr => new FileCompressionTypesChunk(ref chunkInput, header),
                ChunkKind.ABMP => new AfterburnerMapChunk(ref chunkInput, header),
                ChunkKind.FGEI => new FileGzipEmbeddedImageChunk(header),
                
                ChunkKind.imap => new InitialMapChunk(ref chunkInput, header),
                ChunkKind.mmap => new MemoryMapChunk(ref chunkInput, header),
                ChunkKind.KEYPtr => new AssociationTableChunk(ref chunkInput, header),

                ChunkKind.VWCF => new ConfigChunk(ref chunkInput, header),
                ChunkKind.DRCF => new ConfigChunk(ref chunkInput, header),

                //ChunkKind.VWSC => new ScoreChunk(ref chunkInput, header),
                ChunkKind.VWLB => new ScoreLabelChunk(ref chunkInput, header),
                ChunkKind.VWFI => new FileInfoChunk(ref chunkInput, header),
                
                ChunkKind.Lnam => new LingoNameChunk(ref chunkInput, header),
                ChunkKind.Lscr => new LingoScriptChunk(ref chunkInput, header),
                ChunkKind.Lctx => new LingoContextChunk(ref chunkInput, header), //TODO: StackHeight and other differences.
                ChunkKind.LctX => new LingoContextChunk(ref chunkInput, header),
                
                ChunkKind.CASPtr => new CastAssociationTableChunk(ref chunkInput, header),
                ChunkKind.CASt => new CastMemberPropertiesChunk(ref chunkInput, header),
                ChunkKind.MCsL => new MovieCastListChunk(ref chunkInput, header),
                
                //ChunkKind.Cinf => new CastInfoChunk(ref chunkInput, header),
                ChunkKind.SCRF => new ScoreReferenceChunk(ref chunkInput, header),
                ChunkKind.Sord => new SortOrderChunk(ref chunkInput, header),
                ChunkKind.CLUT => new PaletteChunk(ref chunkInput, header),
                ChunkKind.STXT => new StyledTextChunk(ref chunkInput, header),
                
                //ChunkKind.cupt => new CuePointsChunk(ref chunkInput, header),
                ChunkKind.snd => new SoundDataChunk(ref chunkInput, header),

                //ChunkKind.XTRl => new XtraListChunk(ref chunkInput, header),
                //ChunkKind.Fmap => new CastFontMapChunk(ref chunkInput, header),
                
                //ChunkKind.PUBL => new PublishSettingsChunk(ref chunkInput, header),
                ChunkKind.GRID => new GridChunk(ref chunkInput, header),
                ChunkKind.FCOL => new FavoriteColorsChunk(ref chunkInput, header),
                
                ChunkKind.FXmp => new FontMapChunk(ref chunkInput, header),
                ChunkKind.BITD => new BitmapDataChunk(ref chunkInput, header),

                _ => new UnknownChunk(ref chunkInput, header),
            };
        }
    }
}
