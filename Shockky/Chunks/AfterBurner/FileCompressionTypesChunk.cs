using System;
using Shockky.IO;

namespace Shockky.Chunks
{
    public class FileCompressionTypesChunk : ChunkItem
    {
        public short CompressionTypeId { get; set; }
        public int ImageQuality { get; set; }
        public short ImageTypes { get; set; }
        public short DirTypes { get; set; }
        public int CompressionLevel { get; set; }
        public int Speed { get; set; }
        public string Name { get; set; }

        public FileCompressionTypesChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            var decompressedInput = WrapDecompressor(input);
            CompressionTypeId = decompressedInput.ReadBigEndian<short>();
            ImageQuality = decompressedInput.ReadBigEndian<int>();
            ImageTypes = decompressedInput.ReadBigEndian<short>();
            DirTypes = decompressedInput.ReadBigEndian<short>();
            CompressionLevel= decompressedInput.ReadBigEndian<int>();
            Speed = decompressedInput.ReadBigEndian<int>();

            if (CompressionTypeId == 256)
                Name = decompressedInput.ReadNullString();
        }

        public override int GetBodySize()
        {
            throw new NotImplementedException();
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.WriteBigEndian(CompressionTypeId);
            output.WriteBigEndian(ImageQuality);
            output.WriteBigEndian(ImageTypes);
            output.WriteBigEndian(DirTypes);
            output.WriteBigEndian(CompressionLevel);
            output.WriteBigEndian(Speed);

            if (CompressionTypeId == 256)
                output.WriteNullString(Name);
        }
    }
}
