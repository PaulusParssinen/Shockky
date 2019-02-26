using System;
using System.Drawing;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class ConfigChunk : ChunkItem
    {
        public DirectorVersion Version { get; set; }
        public DirectorVersion MovieVersion { get; set; }

        public Rectangle StageRectangle { get; set; }

        public short MinMember { get; }
        public short MaxMember { get; }

        public byte Tempo { get; set; }

        public Color StageBackgroundColor { get; set; } //TODO:

        public short OldDefaultPalette { get; set; }
        public int DefaultPalette { get; set; }

        public short RandomNumber { get; set; } //TODO: Research, file protected when divisible by 0x17
        public bool IsProtected => (RandomNumber % 0x17 == 0);

        public int DownloadFramesBeforePlaying { get; set; }

        public ConfigChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.ReadBigEndian<short>();
            Version = (DirectorVersion)input.ReadBigEndian<ushort>();

            StageRectangle = input.ReadRect();

            MinMember = input.ReadBigEndian<short>(); //Obsolete
            MaxMember = input.ReadBigEndian<short>(); //Obsolete

            Tempo = input.ReadByte();
            Remnants.Enqueue(input.ReadByte());

            byte g = input.ReadByte();
            byte b = input.ReadByte();

            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadBigEndian<short>());

            //https://www.youtube.com/watch?v=sA_eCl4Txzs
            byte r = input.ReadByte();

            StageBackgroundColor = Color.FromArgb(r, g, b);

            Remnants.Enqueue(input.ReadByte());
            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadByte()); //-2 when version < 0x551
            Remnants.Enqueue(input.ReadByte());
            Remnants.Enqueue(input.ReadBigEndian<int>());

            MovieVersion = (DirectorVersion)input.ReadBigEndian<short>();
            Remnants.Enqueue(input.ReadBigEndian<short>());//16?
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadByte());
            Remnants.Enqueue(input.ReadByte());
            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadBigEndian<short>());

            RandomNumber = input.ReadBigEndian<short>();

            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            OldDefaultPalette = input.ReadBigEndian<short>();
            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            DefaultPalette = input.ReadBigEndian<int>(); //TODO:
            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadBigEndian<short>());
            DownloadFramesBeforePlaying = input.ReadBigEndian<int>(); //90
            //Zeros
            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadBigEndian<short>());
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            const short LENGTH = 100;
            output.WriteBigEndian(LENGTH);
            output.WriteBigEndian((ushort)Version);

            output.Write(StageRectangle);

            output.WriteBigEndian(MinMember);
            output.WriteBigEndian(MaxMember);

            output.Write(Tempo);
            output.WriteBigEndian((byte)Remnants.Dequeue());

            output.Write(StageBackgroundColor.G);
            output.Write(StageBackgroundColor.B);

            output.WriteBigEndian((short)Remnants.Dequeue());
            output.WriteBigEndian((short)Remnants.Dequeue());
            output.WriteBigEndian((short)Remnants.Dequeue());

            output.Write(StageBackgroundColor.R);

            output.Write((byte)Remnants.Dequeue());
            output.WriteBigEndian((short)Remnants.Dequeue());
            output.Write((byte)Remnants.Dequeue());
            output.Write((byte)Remnants.Dequeue());
            output.WriteBigEndian((int)Remnants.Dequeue());

            output.WriteBigEndian((short)MovieVersion);
            output.WriteBigEndian((short)Remnants.Dequeue());
            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian((int)Remnants.Dequeue());
            output.Write((byte)Remnants.Dequeue());
            output.Write((byte)Remnants.Dequeue());
            output.WriteBigEndian((short)Remnants.Dequeue());
            output.WriteBigEndian((short)Remnants.Dequeue());

            output.WriteBigEndian(RandomNumber);

            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian((int)Remnants.Dequeue());

            output.WriteBigEndian(OldDefaultPalette);

            output.WriteBigEndian((short)Remnants.Dequeue());
            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian(DefaultPalette);
            output.WriteBigEndian((short)Remnants.Dequeue());
            output.WriteBigEndian((short)Remnants.Dequeue());

            output.WriteBigEndian(DownloadFramesBeforePlaying);
            
            output.WriteBigEndian((short)Remnants.Dequeue());
            output.WriteBigEndian((short)Remnants.Dequeue());
            output.WriteBigEndian((short)Remnants.Dequeue());
            output.WriteBigEndian((short)Remnants.Dequeue());
            output.WriteBigEndian((short)Remnants.Dequeue());
            output.WriteBigEndian((short)Remnants.Dequeue());
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += 100; //TODO:
            return size;
        }
    }
}
