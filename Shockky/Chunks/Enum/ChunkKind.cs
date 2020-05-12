namespace Shockky.Chunks
{
    /// <summary>
    /// Represents TODO
    /// <para/>
    /// See: https://en.wikipedia.org/wiki/FourCC
    /// </summary>
    /// <example>FourCC('R', 'I', 'F', 'X') => (0x52, 0x49, 0x46, 0x58) => 0x52494658</example>
    public enum ChunkKind : int
    {
        Unknown = -1,

        RIFX = 0x52494658,
        XFIR = 0x58464952,

        VERS = 0x56455253,
        FCOL = 0x46434F4C,
        PUBL = 0x5055424C,
        GRID = 0x47524944,
        DRCF = 0x44524346,
        SCRF = 0x53435246,

        // Afterburner
        Fver = 0x46766572,
        Fcdr = 0x46636472,
        ABMP = 0x41424D50,
        FGEI = 0x46474549,
        ILS  = 0x494C5320,

        imap = 0x696d6170,
        mmap = 0x6D6D6170,
        junk = 0x6A756E6B,
        free = 0x66726565,

        Fmap = 0x466D6170,
        FXmp = 0x46586D70,

        XTRl = 0x5854526C,

        MCsL = 0x4D43734C,
        Cinf = 0x43696E66,
        CASt = 0x43415374,
        CASPtr = 0x4341532A, // CAS*
        KEYPtr = 0x4B45592A, // KEY*

        Thum = 0x5468756D,

        BITD = 0x42495444,
        DIB  = 0x44494220,
        CLUT = 0x434C5554,
        ALFA = 0x414C4641,

        snd  = 0x736E6420,
        sndH = 0x736E6448,
        sndS = 0x736E6453,

        // Lingo
        LctX = 0x4C637458,
        Lnam = 0x4C6E616D,
        Lscr = 0x4C736372,

        STXT = 0x53545854,

        XMED = 0x584D4544,
        ediM = 0x6564694D,

        // Score
        SCVW = 0x53435657,
        VWCF = 0x56574346,
        VWFI = 0x56574649,
        VWSC = 0x56575343,
        VWLB = 0x56574C42,
        VWtk = 0x5657746B,

        ccl  = 0x63636C20,
        Sord = 0x536F7264,

        MDFL = 0x4D44464C
    }
}
