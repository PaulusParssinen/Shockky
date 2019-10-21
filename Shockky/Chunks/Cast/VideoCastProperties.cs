using System;
using System.Drawing;

using Shockky.IO;
using Shockky.Chunks.Enum;

namespace Shockky.Chunks.Cast
{
    public class VideoCastProperties : ICastTypeProperties
    {
        public string Type { get; set; }

        public bool Streaming { get; set; }
        public bool HasSound { get; set; }
        public bool PausedAtStart { get; set; }

        public VideoCastFlags Flags { get; set; }
        public byte Framerate { get; set; }
        public Rectangle Rectangle { get; set; }

        public VideoCastProperties()
        { }
        public VideoCastProperties(ShockwaveReader input)
        {
            Type = input.ReadString((int)input.ReadBigEndian<uint>());
            input.Position += 10;

            byte videoFlags = input.ReadByte();
            Streaming = ((videoFlags & 1) == 1);

            videoFlags = input.ReadByte();
            HasSound = ((videoFlags & 1) == 1);
            PausedAtStart = ((videoFlags & 2) == 2);

            Flags = (VideoCastFlags)input.ReadByte();
            input.Position += 3;
            Framerate = input.ReadByte();
            input.Position += 32;
            Rectangle = input.ReadRect();
        }

        public int GetBodySize()
        {
            int size = 0;
            size += sizeof(short);
            size += 10;
            size += sizeof(byte);
            size += sizeof(byte);
            size += sizeof(byte);
            size += 3;
            size += sizeof(byte);
            size += 32;
            size += sizeof(short) * 4;
            return size;
        }

        public void WriteTo(ShockwaveWriter output)
        {
            throw new NotImplementedException(nameof(VideoCastProperties));
            //output.WriteBigEndian(Type);
        }
    }
}
