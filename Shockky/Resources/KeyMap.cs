using Shockky.IO;
using Shockky.Resources.Types;

namespace Shockky.Resources;

public sealed class KeyMap : IShockwaveItem, IResource
{
    private const short ENTRY_SIZE = 12;

    public OsType Kind => OsType.KEYPtr;

    public Dictionary<ResourceId, int> ResourceMap { get; set; }

    public KeyMap()
    {
        ResourceMap = new Dictionary<ResourceId, int>();
    }
    public KeyMap(ref ShockwaveReader input, ReaderContext context)
    {
        input.ReadInt16BigEndian();
        input.ReadInt16BigEndian();
        input.ReadInt32BigEndian();
        int count = input.ReadInt32BigEndian();
        ResourceMap = new Dictionary<ResourceId, int>(count);

        for (int i = 0; i < count; i++)
        {
            int index = input.ReadInt32BigEndian();
            int memberId = input.ReadInt32BigEndian();
            OsType kind = (OsType)input.ReadInt32BigEndian();

            if (!ResourceMap.TryAdd(new ResourceId(kind, memberId), index))
                throw new InvalidOperationException();
        }
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(int);
        size += sizeof(int);
        size += ResourceMap.Count * ENTRY_SIZE;
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.WriteInt16BigEndian(ENTRY_SIZE);
        output.WriteInt16BigEndian(ENTRY_SIZE);
        output.WriteInt32BigEndian(ResourceMap.Count);
        output.WriteInt32BigEndian(ResourceMap.Count);
        foreach ((ResourceId resourceId, int index) in ResourceMap)
        {
            output.WriteInt32BigEndian(index);
            output.WriteInt32BigEndian(resourceId.Id);
            output.WriteInt32BigEndian((int)resourceId.Kind);
        }
    }
}