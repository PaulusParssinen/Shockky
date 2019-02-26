using System.Diagnostics;
using System.IO;

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
            using (var outputMem = new MemoryStream())
            using (var output = new ShockwaveWriter(outputMem))
            {
                WriteTo(output);
                return outputMem.ToArray();
            }
        }

        public abstract int GetBodySize();
        public abstract void WriteTo(ShockwaveWriter output);
    }
}