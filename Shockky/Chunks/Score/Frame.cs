using System;
using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Chunks.Score
{
    public class Frame : ShockwaveItem
    {
        public Channel[] Channels { get; }

        public Frame(ref ShockwaveReader input)
        {
            Debug.WriteLine("## Frame start");

            long endOffset = input.Position + input.ReadInt16();

            if ((endOffset - input.Position) < 0)
                Debugger.Break(); //We at padding of the framez I guess
            
            while ((endOffset - input.Position) > 0)
            {
                short channelLength = input.ReadInt16();
                ushort channelOffset = input.ReadUInt16();
                ReadOnlySpan<byte> data = input.ReadBytes(channelLength);

                Debug.WriteLine($"Channel: {channelOffset / 48} To: {channelOffset} | Len: {channelLength} | Left: {(endOffset - input.Position)}");
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
