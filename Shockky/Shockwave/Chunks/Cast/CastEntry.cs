using System.Diagnostics;
using System.Linq;
using System.Text;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Enum;

namespace Shockky.Shockwave.Chunks.Cast
{
    [DebuggerDisplay("{Name} | File Slot: {FileSlot} | Slot: {Slot}")]
    public class CastEntry : ShockwaveItem
    {
        public int FileSlot { get; set; }
        public int Slot { get; set; }

        public string Name { get; set; }
        public ChunkType Type { get; set; }

        public CastEntry(ShockwaveReader input)
        {
            FileSlot = input.ReadInt32();
            Slot = input.ReadInt32();

	        Name = input.ReadReversedString(4);
            Type = Name.ToChunkType();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += Encoding.ASCII.GetByteCount(Name);
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(FileSlot);
            output.Write(Slot);
            output.Write(Name.Reverse().ToString()); //TODO
        }
    }
}
