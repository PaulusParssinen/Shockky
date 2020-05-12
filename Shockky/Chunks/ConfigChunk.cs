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

        public ConfigChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.ReadInt16();
            Version = (DirectorVersion)input.ReadUInt16();

            StageRectangle = input.ReadRect();

            MinMember = input.ReadInt16(); //Obsolete
            MaxMember = input.ReadInt16(); //Obsolete

            Tempo = input.ReadByte(); // == 0 => 20
            Remnants.Enqueue(input.ReadByte()); //LightSwitch

            byte g = input.ReadByte();
            byte b = input.ReadByte();

            Remnants.Enqueue(input.ReadInt16());
            Remnants.Enqueue(input.ReadInt16());
            Remnants.Enqueue(input.ReadInt16());

            byte r = input.ReadByte();

            StageBackgroundColor = Color.FromArgb(r, g, b);

            Remnants.Enqueue(input.ReadByte());
            Remnants.Enqueue(input.ReadInt16());
            Remnants.Enqueue(input.ReadByte()); //-2 when version < 0x551
            Remnants.Enqueue(input.ReadByte());
            Remnants.Enqueue(input.ReadInt32());

            MovieVersion = (DirectorVersion)input.ReadInt16();
            Remnants.Enqueue(input.ReadInt16());//16?
            Remnants.Enqueue(input.ReadInt32());
            Remnants.Enqueue(input.ReadInt32());
            Remnants.Enqueue(input.ReadInt32());
            Remnants.Enqueue(input.ReadByte());
            Remnants.Enqueue(input.ReadByte());
            Remnants.Enqueue(input.ReadInt16());
            Remnants.Enqueue(input.ReadInt16());

            RandomNumber = input.ReadInt16();

            Remnants.Enqueue(input.ReadInt32());
            Remnants.Enqueue(input.ReadInt32());
            OldDefaultPalette = input.ReadInt16();
            Remnants.Enqueue(input.ReadInt16());
            Remnants.Enqueue(input.ReadInt32());
            DefaultPalette = input.ReadInt32(); //TODO:
            Remnants.Enqueue(input.ReadInt16());
            Remnants.Enqueue(input.ReadInt16());
            DownloadFramesBeforePlaying = input.ReadInt32(); //90
            //Zeros
            Remnants.Enqueue(input.ReadInt16());
            Remnants.Enqueue(input.ReadInt16());
            Remnants.Enqueue(input.ReadInt16());
            Remnants.Enqueue(input.ReadInt16());
            Remnants.Enqueue(input.ReadInt16());
            Remnants.Enqueue(input.ReadInt16());
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            const short LENGTH = 100;
            output.Write(LENGTH);
            output.Write((ushort)Version);

            output.Write(StageRectangle);

            output.Write(MinMember);
            output.Write(MaxMember);

            output.Write(Tempo);
            output.Write((byte)Remnants.Dequeue());

            output.Write(StageBackgroundColor.G);
            output.Write(StageBackgroundColor.B);

            output.Write((short)Remnants.Dequeue());
            output.Write((short)Remnants.Dequeue());
            output.Write((short)Remnants.Dequeue());

            output.Write(StageBackgroundColor.R);

            output.Write((byte)Remnants.Dequeue());
            output.Write((short)Remnants.Dequeue());
            output.Write((byte)Remnants.Dequeue());
            output.Write((byte)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());

            output.Write((short)MovieVersion);
            output.Write((short)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());
            output.Write((byte)Remnants.Dequeue());
            output.Write((byte)Remnants.Dequeue());
            output.Write((short)Remnants.Dequeue());
            output.Write((short)Remnants.Dequeue());

            output.Write(RandomNumber);

            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());

            output.Write(OldDefaultPalette);

            output.Write((short)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());
            output.Write(DefaultPalette);
            output.Write((short)Remnants.Dequeue());
            output.Write((short)Remnants.Dequeue());

            output.Write(DownloadFramesBeforePlaying);
            
            output.Write((short)Remnants.Dequeue());
            output.Write((short)Remnants.Dequeue());
            output.Write((short)Remnants.Dequeue());
            output.Write((short)Remnants.Dequeue());
            output.Write((short)Remnants.Dequeue());
            output.Write((short)Remnants.Dequeue());
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += 100; //TODO:
            return size;
        }
    }
}
