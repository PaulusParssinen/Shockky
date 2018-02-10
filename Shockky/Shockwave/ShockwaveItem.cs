using System.IO;
using Shockky.IO;

namespace Shockky.Shockwave
{
    public abstract class ShockwaveItem
    {
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
