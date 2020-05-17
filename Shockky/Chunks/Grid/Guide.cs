using Shockky.IO;

namespace Shockky.Chunks
{
    public class Guide : ShockwaveItem
    {
        public short Axis { get; set; } //TODO: 1 vertical, 0 horiz
        public short Position { get; set; }

        public Guide(ref ShockwaveReader input)
        {
            Axis = input.ReadInt16();
            Position = input.ReadInt16();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(short);
            size += sizeof(short);
            return size;
        }
        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(Axis);
            output.Write(Position);
        }
    }
}
