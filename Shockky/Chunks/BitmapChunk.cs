﻿using System;

using Shockky.IO;
using Shockky.Chunks.Cast;

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

        public BitmapChunk()
            : base(ChunkKind.BITD)
        { }
        public BitmapChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(ref input, header)
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

            //TODO: A closer look
            Span<byte> outputSpan = TotalWidth * Height < 1024 ? stackalloc byte[TotalWidth * Height] : new byte[TotalWidth * Height];
            var output = new ShockwaveWriter(outputSpan, false);
            var input = new ShockwaveReader(Data.AsSpan());
        
            while (input.IsDataAvailable)
            {
                byte marker = input.ReadByte();
                if (marker >= 128)
                {
                    Span<byte> buffer = stackalloc byte[257 - marker];
                    buffer.Fill(input.ReadByte());
                    output.Write(buffer);
                }
                else output.Write(input.ReadBytes(marker + 1));
            }

            Data = outputSpan.ToArray();
        }
    }
}
