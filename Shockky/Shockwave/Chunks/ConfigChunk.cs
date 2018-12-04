using System;
using System.Drawing;
using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class ConfigChunk : ChunkItem
    {
        public DirectorVersion Version { get; set; }
        
        public ConfigChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.ReadBigEndian<short>();
            Version = (DirectorVersion)input.ReadBigEndian<ushort>();

            var rect = input.ReadRect();

            short minMember = input.ReadBigEndian<short>(); //and max are Obsolete -Docs
            short maxMember = input.ReadBigEndian<short>();

            byte tempo = input.ReadByte();
            Remnants.Enqueue(input.ReadByte());

            byte g = input.ReadByte();
            byte b = input.ReadByte();

            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadBigEndian<short>());

            Remnants.Enqueue(input.ReadBigEndian<short>());

            //ripping from docs
            byte stagePropertiesWhathteufkcdsigfhsdfg = input.ReadByte();

            var stageColorThingy = Color.FromArgb(255, stagePropertiesWhathteufkcdsigfhsdfg, g, b);

            Remnants.Enqueue(input.ReadByte());
            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadByte()); //-2 when version < 0x551
            Remnants.Enqueue(input.ReadByte());
            Remnants.Enqueue(input.ReadBigEndian<int>());

            short movieVersion = input.ReadBigEndian<short>();
            Remnants.Enqueue(input.ReadBigEndian<short>());//16?
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadByte());
            Remnants.Enqueue(input.ReadByte());
            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            short oldDefaultPalette = input.ReadBigEndian<short>();
            Remnants.Enqueue(input.ReadBigEndian<short>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            int defaultPalette = input.ReadBigEndian<int>(); //TODO RGB?
            Remnants.Enqueue(input.ReadBigEndian<short>()); //two bytes
            Remnants.Enqueue(input.ReadBigEndian<short>());
            int downloadFramesBeforePlaying = input.ReadBigEndian<int>(); //90
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

            throw new NotImplementedException();
        }

        public override int GetBodySize()
        {
            int size = 0;
            
            size += 36;
            size += sizeof(byte) * 2;
            return size;
        }
    }
}
