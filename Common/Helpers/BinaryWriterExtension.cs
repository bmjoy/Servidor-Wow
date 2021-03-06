﻿using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.Helpers
{
    public static class BinaryWriterExtension
    {
        public static int WritePackedUInt64(this BinaryWriter binWriter, ulong number)
        {
            var buffer = BitConverter.GetBytes(number);

            byte mask = 0;
            var startPos = binWriter.BaseStream.Position;

            binWriter.Write(mask);

            for (var i = 0; i < 8; i++)
            {
                if (buffer[i] != 0)
                {
                    mask |= (byte)(1 << i);
                    binWriter.Write(buffer[i]);
                }
            }

            var endPos = binWriter.BaseStream.Position;

            binWriter.BaseStream.Position = startPos;
            binWriter.Write(mask);
            binWriter.BaseStream.Position = endPos;

            return (int)(endPos - startPos);
        }

        public static void WriteNullByte(this BinaryWriter writer, uint count)
        {
            for (uint i = 0; i < count; i++)
            {
                writer.Write((byte)0);
            }
        }

        public static void WriteCString(this BinaryWriter writer, string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input + '\0');
            writer.Write(data);
        }

        public static void WriteBytes(this BinaryWriter writer, byte[] data, int count = 0)
        {
            if (count == 0)
                writer.Write(data);
            else
                writer.Write(data, 0, count);
        }

        public static void WriteBitArray(this BinaryWriter writer, BitArray buffer, int len)
        {
            byte[] bufferarray = new byte[Convert.ToByte((buffer.Length + 8) / 8) + 1];
            buffer.CopyTo(bufferarray, 0);

            WriteBytes(writer, bufferarray.ToArray(), len);
        }
    }
}
