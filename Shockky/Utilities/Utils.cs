using System;
using System.Diagnostics;
using Shockky.Shockwave.Chunks.Enum;

namespace Shockky.Utilities
{
    public static class Utils
    {
        public static string Reverse(string s)
        {
            var charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static ChunkType ToChunkType(this string typeName) //Top fucking notch =DD
        {
            try
            {
                return (ChunkType) Enum.Parse(typeof(ChunkType), typeName.Replace("*", "Star"));
            }
            catch (Exception)
            {
                Debug.WriteLine("Unknown TypeName: " + typeName);
                return ChunkType.Unknown;
            }
        }

        //Yeah ffs I hate this bullshit
        public static int Swap(int value)
        {
            var b1 = (value >> 0) & 0xff;
            var b2 = (value >> 8) & 0xff;
            var b3 = (value >> 16) & 0xff;
            var b4 = (value >> 24) & 0xff;

            return b1 << 24 | b2 << 16 | b3 << 8 | b4 << 0;
        }

        public static short Swap(short value)
        {
            return (short)(((0xFF & value) << 8) | (value & 0xFF00) >> 8);
        } 
    }
}
