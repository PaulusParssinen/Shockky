using Shockky.IO;

namespace Shockky.Collections;

/// <summary>
/// Represents a Director serialized list structure with a typed header and items.
/// </summary>
/// <typeparam name="THeader">The list header type.</typeparam>
/// <typeparam name="TItem">The list item element type.</typeparam>
public class ShockwaveList<THeader, TItem> : List<TItem>, IShockwaveItem<ShockwaveList<THeader, TItem>>
    where THeader : struct, IListHeader, IShockwaveItem<THeader>
    where TItem : IShockwaveItem<TItem>
{
    private THeader _header;

    public ShockwaveList()
    {
        _header = new THeader();
    }

    public ShockwaveList(int capacity) : base(capacity)
    {
        _header = new THeader();
    }

    public ShockwaveList(IEnumerable<TItem> collection) : base(collection)
    {
        _header = new THeader();
    }

    public ShockwaveList(ref ShockwaveReader input, ReaderContext context)
    {
        _header = THeader.Read(ref input, context);
        int count = _header.Count;
        Capacity = count;
        for (int i = 0; i < count; i++)
        {
            Add(TItem.Read(ref input, context));
        }
    }

    public static ShockwaveList<THeader, TItem> Read(ref ShockwaveReader input, ReaderContext context) => new(ref input, context);

    public int GetBodySize(WriterOptions options)
    {
        int size = _header.GetBodySize(options);
        for (int i = 0; i < Count; i++)
        {
            size += this[i].GetBodySize(options);
        }
        return size;
    }

    public void WriteTo(ref ShockwaveWriter output, WriterOptions options)
    {
        _header.Count = Count;
        _header.WriteTo(ref output, options);

        for (int i = 0; i < Count; i++)
        {
            this[i].WriteTo(ref output, options);
        }
    }
}

/// <summary>
/// Represents a Director 5+ serialized list structure with a standard <see cref="ListHeaderV5"/> header.
/// </summary>
/// <typeparam name="TItem">The list item element type.</typeparam>
public class ShockwaveList<TItem> : ShockwaveList<ListHeaderV5, TItem>
    where TItem : IShockwaveItem<TItem>
{
    public ShockwaveList() : base() { }
    public ShockwaveList(int capacity) : base(capacity) { }
    public ShockwaveList(IEnumerable<TItem> collection) : base(collection) { }
    public ShockwaveList(ref ShockwaveReader input, ReaderContext context) : base(ref input, context) { }

    public static new ShockwaveList<TItem> Read(ref ShockwaveReader input, ReaderContext context) => new(ref input, context);
}