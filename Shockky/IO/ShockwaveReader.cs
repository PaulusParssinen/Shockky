using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shockky.IO.Conversion;
using Shockky.Shockwave;
using Shockky.Utilities;
using static Shockky.Utilities.Utils;

namespace Shockky.IO
{
    public class ShockwaveReader : EndianBinaryReader
    {
        /* TODO:s
         * - Implement jumping
         * 
         */

        public long Length => BaseStream.Length;
        public bool IsDataAvailable => Position < Length;

        public long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        protected int BitPosition { get; set; }
        protected byte BitContainer { get; set; }

        public ShockwaveReader(byte[] data)
            : this(new MemoryStream(data))
        { }
        public ShockwaveReader(Stream stream)
            : base(EndianBitConverter.Little, stream)
        { }

        public ShockwaveReader(ShockwaveReader input, int length)
            : this(new MemoryStream(input.ReadBytes(length)))
        { }

        public ShockwaveReader(EndianBitConverter endianness, Stream stream)
            : base(endianness, stream)
        { }

        public float ReadInt64(bool swapEndianness)
        {
            byte[] floatBites = ReadBytes(8);

            if(swapEndianness)
                Array.Reverse(floatBites);

            return BitConverter.ToInt64(floatBites, 0);
        }
        public int ReadInt32(bool swapEndianness)
        {
            int val = ReadInt32();
            return swapEndianness ? Swap(val) : val;
        }
        public short ReadInt16(bool swapEndianness)
        {
            short val = ReadInt16();
            return swapEndianness ? Swap(val) : val;
        }

        //TODO: Absolute bullshit
        public string ReadString(int length = -1, bool followEndianness = false)
        {
            if (length < 0)
                length = ReadByte();

            string value = Encoding.GetString(ReadBytes(length));

            return followEndianness ? Reverse(value) : value;
        }

        public List<T> ReadList<T>(int count, int offset = 0)
            where T : ShockwaveItem
        {
            if(offset > 0) //TODO: auto entry count reading niggas? nah i guess
                Position = offset;

            //I wanna die btw
            var newItem = ObjectCreator.GetItemConstructor<T>();

            var list = new List<T>(count);
            for (int i = 0; i < count; i++)
            { 
                list.Add(newItem(this));
            }

            return list;
        }

        public List<int> ReadIntList(int count, int offset = 0, bool bigEndian = true)
        {
            if (offset > 0)
                Position = offset;

            var list = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(ReadInt32(bigEndian));
            }
            return list;
        }
        public List<short> ReadShortList(int count, int offset = 0, bool bigEndian = true)
        { 
            if(offset > 0)
                Position = offset;

            var list = new List<short>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(ReadInt16(bigEndian));
            }

            return list;
        }
        public List<string> ReadStringList(int count, int stringLength = 0, int offset = 0)
        {
            if (offset > 0)
                Position = offset;

            var list = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(ReadString(stringLength));
            }

            return list;
        }

        public List<string> MapNameList(int count, int offset, List<string> nameList, bool resetPosition = false, bool bigEndian = true) //Absolutely horrible
        {
            long ogPos = Position;

            if (offset > 0)
                Position = offset;

            var list = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                short index = ReadInt16(bigEndian);

                if(index == -1) continue;

                list.Add(index > nameList.Count
                    ? "OUTOFBOUNDS" : nameList[index]); //TODO: GOD FUCKING DAMN IT FUCK YOU SHOCKWAVE
            }

            if(resetPosition)
                Position = ogPos;

            return list;
        }
    }
}
