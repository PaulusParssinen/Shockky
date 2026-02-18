using System.CommandLine;
using System.Runtime.InteropServices;

using Shockky;
using Shockky.Resources;
using Shockky.Resources.Cast;
using Shockky.Resources.Types;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

Console.Title = "Shockky.Sandbox";

//TODO: Verbose and Quiet levels, and rest of the resources of course
var inputArgument = new Argument<IEnumerable<System.IO.FileInfo>>("input")
{
    Arity = ArgumentArity.OneOrMore,
    Description = "Director movie (.d[ixc]r) or external cast (.c[sxc]t) file(s)."
}.AcceptExistingOnly();

var outputOption = new Option<DirectoryInfo>("--output", "-o")
{
    Description = "Directory for the extracted resources",
    DefaultValueFactory = _ => new DirectoryInfo("Output")
}.AcceptLegalFilePathsOnly();

var rootCommand = new RootCommand()
{
    inputArgument,
    outputOption
};

rootCommand.SetAction(parseResult =>
{
    var input = parseResult.GetRequiredValue(inputArgument);
    var output = parseResult.GetRequiredValue(outputOption);

    HandleExtractCommand(input, output);
});

return rootCommand.Parse(args).Invoke();

static IReadOnlyDictionary<int, System.Drawing.Color[]> ReadPalettes()
{
    static System.Drawing.Color[] ReadPalette(string fileName)
    {
        using var fs = File.OpenRead(fileName);
        using var input = new BinaryReader(fs);

        input.ReadChars(4);
        input.ReadInt32();

        input.ReadChars(4);

        input.ReadChars(4);
        input.ReadInt32();
        input.ReadInt16();

        System.Drawing.Color[] colors = new System.Drawing.Color[input.ReadInt16()];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = System.Drawing.Color.FromArgb(
                red: input.ReadByte(),
                green: input.ReadByte(),
                blue: input.ReadByte());

            input.ReadByte();
        }
        return colors;
    }

    return new Dictionary<int, System.Drawing.Color[]>
    {
        { -1, ReadPalette("Palettes/mac.pal") },
        { -2, ReadPalette("Palettes/rainbow.pal") },
        { -3, ReadPalette("Palettes/grey.pal") },
        { -4, ReadPalette("Palettes/pastels.pal") },
        { -5, ReadPalette("Palettes/vivid.pal") },
        { -6, ReadPalette("Palettes/ntsc.pal") },
        { -7, ReadPalette("Palettes/metallic.pal") },
        { -8, ReadPalette("Palettes/web216.pal") },
        { -9, null }, //TODO: "Palettes/VGA.pal"
        { -101, ReadPalette("Palettes/windir4.pal") },
        { -102, ReadPalette("Palettes/win.pal") }
    };
}

static void HandleExtractCommand(IEnumerable<System.IO.FileInfo> input, DirectoryInfo output)
{
    output.Create();

    //Load the built-in system palettes
    IReadOnlyDictionary<int, System.Drawing.Color[]> systemPalettes = ReadPalettes();

    foreach (var file in input)
    {
        //TODO: Seperate different resource types.
        DirectoryInfo fileOutputDirectory = output.CreateSubdirectory(file.Name);

        Console.Write($"Disassembling file \"{file.Name}\"..");

        var shockwaveFile = ShockwaveFile.Read(file.FullName);

        if (shockwaveFile.Resources.Values
            .FirstOrDefault(c => c.Kind == OsType.KEYPtr) is not KeyMapResource associationTable)
        {
            Console.WriteLine(nameof(KeyMapResource) + " was not found!");
            continue;
        }
        if (shockwaveFile.Resources.Values
            .FirstOrDefault(c => c.Kind == OsType.CASPtr) is not CastMapResource castAssociationTable)
        {
            Console.WriteLine(nameof(CastMapResource) + " was not found!");
            continue;
        }
        Console.Write("Extracting bitmaps..");

        //TODO: This is currently just hacks all around. Shockky should have these lookups abstracted away.
        foreach ((ResourceId resourceId, int index) in associationTable.ResourceMap)
        {
            if (resourceId.Kind != OsType.BITD) continue;

            var member = shockwaveFile.Resources[resourceId.Id] as CastMemberPropertiesResource;
            var bitmapData = shockwaveFile.Resources[index] as BitmapDataResource;

            if (member?.Properties is not BitmapCastProperties bitmapProperties)
                continue;

            //TODO: external castlibs
            if (bitmapProperties.PaletteRef.CastLib > 0)
                continue;

            if (bitmapProperties.Rectangle.IsEmpty)
                continue;

            string outputFilePath = Path.Combine(fileOutputDirectory.FullName, member.Metadata?.Entries.Name ?? resourceId.Id.ToString());

            if (bitmapProperties.PaletteRef.MemberNum > 0 && bitmapProperties.PaletteRef.MemberNum < castAssociationTable.Members.Length)
            {
                continue;
                //TODO:

                //int paletteOwnerIndex = castAssociationTable.Members[bitmapProperties.PaletteRef.MemberNum];
                //if (shockwaveFile[paletteOwnerIndex] is not CastMemberProperties paletteMember)
                //    continue;
                //
                //var paletterResId = new ResourceId(ResourceKind.CLUT, paletteOwnerIndex);
                //
                //if (associationTable.ResourceMap.TryGetValue(paletterResId, out int paletteChunkIndex))
                //{
                //    var palette = shockwaveFile[paletteChunkIndex] as Palette;
                //    bitmapData.PopulateMedia(bitmapProperties);
                //    if (!TryExtractBitmapResource(fileOutputDirectory, outputFileName, bitmapData, palette.Colors))
                //        continue;
                //}
            }
            else if (systemPalettes.TryGetValue(bitmapProperties.PaletteRef.MemberNum - 1, out System.Drawing.Color[] palette))
            {
                if (!TryExtractBitmapResource(outputFilePath, bitmapProperties, bitmapData, palette))
                    continue;
            }
            Console.WriteLine($"({bitmapProperties.PaletteRef.CastLib}, {bitmapProperties.PaletteRef.MemberNum}) {member.Metadata?.Entries.Name}:");
            Console.WriteLine($"    BitDepth: {bitmapProperties.BitDepth}");
        }
        Console.WriteLine();
        Console.WriteLine("Done!");
    }
}

static bool TryExtractBitmapResource(string outputFilePath,
    BitmapCastProperties properties,
    BitmapDataResource bitmapData,
    System.Drawing.Color[] palette)
{
    static int CalcStride(int width, int bitdepth)
        => (bitdepth switch
        {
            1 => (width + 7) / 8,
            2 => (width + 3) / 4,
            4 => (width + 1) / 2,
            8 => width,
            16 => width * 2,
            24 => width * 3,
            32 => width * 4,

            _ => throw new ArgumentException(nameof(bitdepth))
        } + 1) & ~1;

    //TODO: Properly render flags etc.
    int stride = properties.Stride;
    int width = Math.Min(properties.Rectangle.Width, stride);
    int bufferLength = stride * width;

    Span<byte> buffer = bufferLength <= 1024 ? stackalloc byte[bufferLength] : new byte[bufferLength];

    if (!bitmapData.TryDecompress(properties, buffer, out int decompressed))
        return false;

    using var image = new Image<Rgba32>(properties.Rectangle.Width, properties.Rectangle.Height);
    for (int y = 0; y < properties.Rectangle.Height; y++)
    {
        Span<byte> row = buffer.Slice(y * stride, stride);

        if (properties.BitDepth == 32) //TODO: Can't get this right yet, probably wrong PixelFormat
        {
            return false;

            //Span<Bgra32> pixels = MemoryMarshal.Cast<byte, Bgra32>(row);
            //
            //for (int x = 0; x < bitmap.Width; x++)
            //{
            //    image[x, y] = pixels[x];
            //}
        }
        else if (properties.BitDepth == 16)
        {
            for (int x = 0; x < width; x++)
            {
                // Wrong
                int pixelColor = MemoryMarshal.Read<short>(row.Slice(x * 2));
                byte r = (byte)((pixelColor & 0b111100000000) >> 8);
                byte g = (byte)((pixelColor & 0b000011110000) >> 4);
                byte b = (byte)(pixelColor & 0b000000001111);
                image[x, y] = new Rgba32(r, g, b);
            }
        }
        else if (properties.BitDepth == 8)
        {
            for (int x = 0; x < width; x++)
            {
                System.Drawing.Color pixelColor = palette[row[x]];
                image[x, y] = new Rgba32(pixelColor.R, pixelColor.G, pixelColor.B);
            }
        }
        else if (properties.BitDepth == 4)
        {
            return false;

            // Wrong
            //for (int x = 0; x < width; x++)
            //{
            //    System.Drawing.Color pixelColor = palette[row[x] >> 4];
            //    System.Drawing.Color secondPixelColor = palette[row[x] & 0xF];
            //
            //    //image[x, y] = new Bgra32(pixelColor.R, pixelColor.G, pixelColor.B);
            //}
        }
        else if (properties.BitDepth == 1)
        {
            // Also wrong a bit
            for (int x = 0; x < properties.Rectangle.Width;)
            {
                for (int c = 0; c < 8 && x < properties.Rectangle.Width; c++, x++)
                {
                    int p = row[x / 8] & (1 << (7 - c));
                    image[x, y] = new Rgba32(p, p, p);
                }
            }
        }
    }

    using var fs = File.Create(outputFilePath + ".png");
    image.SaveAsPng(fs);

    return true;
}