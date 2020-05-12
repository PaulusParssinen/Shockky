using System;

using Shockky.IO;

namespace Shockky.Chunks.Score
{
    public class Channel : ShockwaveItem
    {
        public byte ForeColor { get; set; }
        public byte BackColor { get; set; }

        public short LocV { get; set; }
        public short LocH { get; set; }

        public short Height { get; set; }
        public short Width { get; set; }

        public Channel(ref ShockwaveReader input)
        { }

        public override int GetBodySize()
        {
            throw new NotImplementedException();
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }
    }
}
