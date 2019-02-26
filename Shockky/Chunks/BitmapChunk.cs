using System.IO;
using System;

using Shockky.Chunks.Enum;
using Shockky.Chunks.Cast;
using Shockky.IO;

namespace Shockky.Chunks
{
    public class BitmapChunk : BinaryDataChunk, ICastMemberMediaChunk<BitmapCastProperties>
    {
        public byte BitDepth { get; set; }
        public BitmapFlags Flags { get; set; }

        //TODO: Check if matches the one calculated with bitdepth
        //TODO: naming PixelsPerRow/ScanLine
        public int TotalWidth { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public BitmapChunk(ShockwaveReader input, ChunkHeader header)
            : base(input, header)
        { }

        public void PopulateMedia(BitmapCastProperties properties)
        {
            BitDepth = properties.BitDepth;

            Flags = properties.Flags;

            TotalWidth = properties.TotalWidth;
            Width = properties.Rectangle.Width;
            Height = properties.Rectangle.Height;

            if (Data.Length == TotalWidth * Height)
                return;

            using (var ms = new MemoryStream(TotalWidth * Height))
            using (var output = new ShockwaveWriter(ms))
            using (var input = new ShockwaveReader(Data))
            {
                while (input.IsDataAvailable)
                {
                    byte marker = input.ReadByte();
                    if (marker >= 128)
                    {
                        Span<byte> buffer = stackalloc byte[257 - marker];
                        buffer.Fill(input.ReadByte());
                        output.Write(buffer);
                    }
                    else output.Write(input.ReadBytes(++marker));
                }

                Data = ms.ToArray();
            }
        }
    }
}
