﻿using System.IO;
using System.Text;
using Common.Globals;

namespace Common.Network
{
    public class PacketServer : BinaryWriter
    {
        public int Opcode;

        public PacketServer(int opcode) : base(new MemoryStream())
        {
            Opcode = opcode;
        }

        public PacketServer(AuthCMD authOpcode) : this((byte)authOpcode) { }

        public PacketServer(RealmCMD realmOpcode) : this((int)realmOpcode) { }

        public byte[] Packet => (BaseStream as MemoryStream)?.ToArray();

        /// <summary>
        /// Writes a C-style string. (Ends with a null terminator)
        /// </summary>
        /// <param name="input">The input.</param>
        public void WriteCString(string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input + '\0');
            Write(data);
        }

        /// <summary>
        /// Writes a null byte to the stream.
        /// </summary>
        public void Write()
        {
            Write((byte)0x0);
        }
    }
}
