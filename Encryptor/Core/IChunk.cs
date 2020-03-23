using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Encryptor.Crypto;

namespace Encryptor.Core
{
    internal interface IChunk
    {
        byte[] Hash { get; }
        byte[] Blob { get; }
        int Size { get; }
    }

    [Serializable]
    internal class Chunk : IChunk
    {
        public Chunk(byte[] buffer, byte[] hash)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (buffer.Length > CoreConstants.Chunk512Size)
                throw new ArgumentException($"Constant should be less then {CoreConstants.Chunk512Size}");

            Blob = buffer;
            Hash = hash;
        }

        public byte[] Hash { get; }
        public byte[] Blob { get; }
        public int Size => Blob.Length;
    }

    internal class Chunker
    {
        public Chunker()
        {

        }

        internal IEnumerable<Memory<byte>> BuildChunks(Stream stream, CancellationToken token)
        {
            var blockSize = CoreConstants.Chunk512Size;

            var buffer = new Memory<byte>();
            stream.Position = 0;

            ArrayPool<byte> pool = ArrayPool<byte>.Create(blockSize,1);
            var readed = 0;
            while (stream.Length != readed)
            {
                var arr = pool.Rent(blockSize);

                int bytesLeft = (int)stream.Length - readed;
                int toRead = bytesLeft > blockSize ? blockSize : bytesLeft;
                readed +=  stream.Read(arr, readed, toRead);

                yield return new Memory<byte>(arr);

            }

        }
    }
}