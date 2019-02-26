using System;
using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Chunks.Score
{
    public class Frame : ShockwaveItem
    {
        public Channel[] Channels { get; }

        public Frame(ShockwaveReader input)
        {
            long endOffset = input.Position + input.ReadBigEndian<short>();

            if ((endOffset - input.Position) < 0)
                Debugger.Break(); //We at padding of the framez I guess
            
            while ((endOffset - input.Position) > 0)
            {
                short channelLength = input.ReadBigEndian<short>();
                ushort channelOffset = input.ReadBigEndian<ushort>();
                byte[] data = input.ReadBytes(channelLength);

                Debug.WriteLine($"Channel??: {channelOffset / 48} To: {channelOffset} | Len: {channelLength} | Left: {(endOffset - input.Position)}");
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(short);

            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }
    }
}
