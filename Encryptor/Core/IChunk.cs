using System;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Encryptor.Core
{
    internal interface IChunk
    {
        byte[] Hash { get; }
        byte[] Blob { get; }
        int DataLen { get; }
    }

    [Serializable]
    internal class Chunk : IChunk
    {
        public Chunk(byte[] buffer, byte[] hash, int dataLen)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
//            if (buffer.Length > CoreConstants.Chunk512Size)
//                throw new ArgumentException($"Constant should be less then {CoreConstants.Chunk512Size}");

            Blob = buffer;
            Hash = hash;
            DataLen = dataLen;
        }

        public byte[] Hash { get; }
        public byte[] Blob { get; }
        public int DataLen { get; }
    }
}
