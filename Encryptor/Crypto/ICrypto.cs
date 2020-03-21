using System;
using System.Threading;
using System.Threading.Tasks;

namespace Encryptor.Crypto
{
    /// <summary> Base crypto functionality </summary>
    internal interface ICrypto
    {
        ReadOnlyMemory<byte> Encrypt(ReadOnlyMemory<byte> buffer);
        ReadOnlyMemory<byte> Decrypt(ReadOnlyMemory<byte> buffer);
        Task<ReadOnlyMemory<byte>> EncryptAsync(ReadOnlyMemory<byte> buffer, CancellationToken token);
        Task<ReadOnlyMemory<byte>> DecryptAsync(ReadOnlyMemory<byte> buffer, CancellationToken token);
    }
}