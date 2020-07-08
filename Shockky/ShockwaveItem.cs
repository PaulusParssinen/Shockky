using System.Buffers;
using System.Diagnostics;

using Shockky.IO;

namespace Shockky
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract class ShockwaveItem
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual string DebuggerDisplay => "{" + ToString() + "}";

        public byte[] ToArray()
        {
            var arrayWriter = new ArrayBufferWriter<byte>(GetBodySize());
            WriteTo(arrayWriter);

            return arrayWriter.WrittenSpan.ToArray();
        }

        public void WriteTo(IBufferWriter<byte> output)
        {
            int size = GetBodySize();
            var writer = new ShockwaveWriter(output.GetSpan(size), isBigEndian: true); //TODO: Endianness
            
            WriteTo(writer);
            output.Advance(size);
        }

        public abstract int GetBodySize();
        public abstract void WriteTo(ShockwaveWriter output);
    }
}