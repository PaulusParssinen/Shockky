using System;
using System.Drawing;
using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class DRCFChunk : ChunkItem
    {
        public string VersionHex { get; set; }
        
        public DRCFChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Debug.Assert(header.Length == 100, "HMMMM");

            short length = input.ReadBigEndian<short>();
            ushort version = input.ReadBigEndian<ushort>();
            VersionHex = version.ToString("X");

            var rect = input.ReadRect(true);

            short minMember = input.ReadBigEndian<short>(); //and max are Obsolete -Docs
            short maxMember = input.ReadBigEndian<short>();

            byte tempo = input.ReadByte();
            byte unk1 = input.ReadByte();

            byte g = input.ReadByte();
            byte b = input.ReadByte();

            short unk2 = input.ReadBigEndian<short>();
            short unk3 = input.ReadBigEndian<short>();

            short unk4 = input.ReadBigEndian<short>();

            //ripping from docs
            byte stagePropertiesWhathteufkcdsigfhsdfg = input.ReadByte();

            var stageColorThingy = Color.FromArgb(255, stagePropertiesWhathteufkcdsigfhsdfg, g, b);

            byte unk5 = input.ReadByte();
            short unk6 = input.ReadBigEndian<short>();
            byte unk7 = input.ReadByte(); //-2 when version < 0x551
            byte unk8 = input.ReadByte();
            int unk9 = input.ReadBigEndian<int>();

            short movieVersion = input.ReadBigEndian<short>();
            short unk10 = input.ReadBigEndian<short>();//16?
            int unk11 = input.ReadBigEndian<int>();
            int unk12 = input.ReadBigEndian<int>();
            int unk13 = input.ReadBigEndian<int>();
            byte unk14 = input.ReadByte();
            byte unk15 = input.ReadByte();
            short unk16 = input.ReadBigEndian<short>();
            short unk17 = input.ReadBigEndian<short>();
            short unk18 = input.ReadBigEndian<short>();
            int unk19 = input.ReadBigEndian<int>();
            int unk20 = input.ReadBigEndian<int>();
            short oldDefaultPalette = input.ReadBigEndian<short>();
            short unk21 = input.ReadBigEndian<short>();
            int unk22 = input.ReadBigEndian<int>();
            int defaultPalette = input.ReadBigEndian<int>(); //TODO RGB?
            short unk23 = input.ReadBigEndian<short>(); //two bytes
            short unk24 = input.ReadBigEndian<short>();
            int downloadFramesBeforePlaying = input.ReadBigEndian<int>(); //90
            //Zeros
            short unk25 = input.ReadBigEndian<short>();
            short unk26 = input.ReadBigEndian<short>();
            short unk27 = input.ReadBigEndian<short>();
            short unk28 = input.ReadBigEndian<short>();
            short unk29 = input.ReadBigEndian<short>();
            short unk30 = input.ReadBigEndian<short>();
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
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
