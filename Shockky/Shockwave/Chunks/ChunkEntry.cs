using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    [DebuggerDisplay("{Header.Name} | Length: {Header.Length} | Offset: {Offset}")]
    public class ChunkEntry : ShockwaveItem
    {
        public ChunkHeader Header { get; set; }

        public int Offset { get; set; }
        public short Padding { get; set; }
        public short Unknown { get; set; }
        public int Link { get; set; }

        public ChunkEntry(ChunkHeader header)
        {
            Header = header;
        }
        public ChunkEntry(ShockwaveReader input, int id)
            : this(new ChunkHeader(input))
        {
            Header.Id = id;

            Offset = input.ReadInt32();
            Padding = input.ReadInt16();
            Unknown = input.ReadInt16();
            Link = input.ReadInt32();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += Header.GetBodySize();
            size += sizeof(int);
            size += sizeof(short);
            size += sizeof(short);
            size += sizeof(int);
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            Header.WriteTo(output);

            output.Write(Offset);
            output.Write(Padding);
            output.Write(Unknown);
            output.Write(Link);
        }
    }
}