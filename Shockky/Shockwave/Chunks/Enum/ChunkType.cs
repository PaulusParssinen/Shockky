using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Shockky.Shockwave.Chunks.Enum
{
    [SuppressMessage("ReSharper", "InconsistentNaming")] //fek yoy
    public enum ChunkType
    {
        Unknown = -1,
        RIFX,
        VERS,
        FCOl,
        PUBL,
        GRID,
        DRCF,
        SCRF,

        imap,
        mmap,
        junk,
        free,
        Fmap,
        FXmp,

        XTRl,

        MCsL,
        Cinf,
        CASt,
        CASStar,
        KEYStar,

        BITD,
        DIB,
        CLUT,
        ALFA,

        snd,

        LctX,
        Lnam,
        Lscr,

        STXT,

        XMED,

        ediM,

        SCVW,
        VWFI,
        VWSC,
        VWLB,
        VWtk,

        ccl,
        Sord
    }

	public static class ChunkExtensions
	{
		public static ChunkType ToChunkType(this string typeName)
		{
			try
			{
				return (ChunkType)System.Enum.Parse(typeof(ChunkType), typeName.Replace("*", "Star"));
			}
			catch (Exception)
			{
				Debug.WriteLine("Unknown ChunkType name: " + typeName);
				return ChunkType.Unknown;
			}
		}
	}
}
