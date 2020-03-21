using System;
using System.Security.Authentication;
using Encryptor.Crypto;

namespace Encryptor.Core
{
    internal interface IChunk
    {
        byte[] Hash { get; }
        byte[] Blob { get; }
        int Size { get; }
    }

    internal class Chunk : IChunk
    {
        public Chunk(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (buffer.Length > CoreConstants.Chunk512Size)
                throw new ArgumentException($"Constant should be less then {CoreConstants.Chunk512Size}");

            Blob = buffer;
            Hash = HashingUtils.ComputeHash(Blob, HashAlgorithmType.Sha1);
        }

        public byte[] Hash { get; }
        public byte[] Blob { get; }
        public int Size => Blob.Length;
    }
}