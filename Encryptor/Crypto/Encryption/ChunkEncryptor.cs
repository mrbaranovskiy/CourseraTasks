using System;
using System.Security.Cryptography;
using Encryptor.Crypto.Keys;
using Encryptor.Crypto.Encryption;

namespace Encryptor.Crypto
{
    internal class ChunkEncryptor : CryptoManagerBase
    {
        public ChunkEncryptor(IKeyProvider provider,
            SymmetricAlgorithm algorithm)
            : base(provider, algorithm)
        {
        }

        public virtual ReadOnlyMemory<byte> Decrypt(ReadOnlyMemory<byte> data, int targetSize)
        {
            var decrypted = base.Decrypt(data);
            return decrypted.Slice(0, targetSize);
        }
    }
}