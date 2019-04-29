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
            using (var ms = new MemoryStream())
            using (var output = new ShockwaveWriter(ms))
            {
                WriteTo(output);
                return ms.ToArray();
            }
        }

        public abstract int GetBodySize();
        public abstract void WriteTo(ShockwaveWriter output);
    }
}