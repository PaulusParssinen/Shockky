using System.Collections.Generic;

namespace Shockky.Shockwave.Chunks.Interface
{
    public interface IChunkEntryMap
    {
        List<IChunkEntry> Entries { get; set; }
    }
}
