namespace Shockky.Resources.Cast;

[ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
public sealed partial class ScriptCastProperties : IMemberProperties, IShockwaveItem<ScriptCastProperties>
{
    public ScriptKind Kind { get; set; }
}