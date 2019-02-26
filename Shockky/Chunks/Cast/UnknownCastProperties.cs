using System;
using System.Drawing;

using Shockky.IO;

namespace Shockky.Chunks.Cast
{
    public class UnknownCastProperties : ShockwaveItem, ICastTypeProperties
    {
        public byte[] Data { get; set; }

        public UnknownCastProperties(ShockwaveReader input, int length)
        {
            Data = input.ReadBytes(length);
        }

        public override int GetBodySize() => Data.Length;
        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(Data);
        }

        public Rectangle Rectangle
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }
    }
}
