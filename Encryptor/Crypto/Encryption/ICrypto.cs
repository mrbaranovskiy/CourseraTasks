using System;
using System.Threading;
using System.Threading.Tasks;

namespace Encryptor.Crypto.Encryption
{
    /// <summary> Base crypto functionality </summary>
    internal interface ICrypto
    {
        ReadOnlyMemory<byte> Encrypt(ReadOnlyMemory<byte> buffer);
        ReadOnlyMemory<byte> Decrypt(ReadOnlyMemory<byte> buffer);
        Task<ReadOnlyMemory<byte>> EncryptAsync(ReadOnlyMemory<byte> buffer, CancellationToken token);
        Task<ReadOnlyMemory<byte>> DecryptAsync(ReadOnlyMemory<byte> buffer, CancellationToken token);
    }

    /// <summary>
    /// Provides the hash ans checksum calculation service
    /// </summary>
    internal interface IHashService
    {
        byte[] Hash(byte[] arr);
        byte[] Hash(string str);
    }
    
    /// <summary>
    /// Sha1 hashing service
    /// </summary>
    internal class Sha1HashService : IHashService
    {
        public byte[] Hash(byte[] arr) => HashingUtils.ComputeHash(arr);

        public byte[] Hash(string str)
        {
            throw new NotImplementedException();
        }
    }
}