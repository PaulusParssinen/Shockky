using System;
using System.Collections.Generic;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Enum;

namespace Shockky.Shockwave.Chunks
{
    public class CastChunk : ChunkItem
    {
        public CastChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            var castType = (CastType)input.ReadInt32(true);
            int castDataLength = input.ReadInt32(true);
            int castEndDataLength = input.ReadInt32(true);

            if (castDataLength > 0)
            {
                int bitFieldLength = input.ReadInt32(true);
                byte[] bitField = input.ReadBytes(bitFieldLength - 4);

                short castFieldCount = input.ReadInt16(true);

                var offsets = new int[castFieldCount];

                for (short i = 0; i < castFieldCount; i++)
                {
                    offsets[i] = input.ReadInt32(true);
                }

                int castFieldDataLength = input.ReadInt32(true);

                if (castType == CastType.Script)
                {
                    string name = input.ReadString();
                    string comment = string.Empty;

                    int dataLeft = offsets[castFieldCount - 1] - name.Length - 1;

                    if (dataLeft != 0)
                        comment = input.ReadString(dataLeft - 2);

                   // Console.WriteLine($"Script Name: {name} | Comment: {(comment == string.Empty ? "None" : comment)}");
                }
            }
        }
    }
}
