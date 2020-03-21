using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Encryptor.Crypto
{
    internal abstract class CryptoManagerBase : ICrypto
    {
        private readonly IKeyProvider _provider;
        private readonly SymmetricAlgorithm _algorithm;

        /// <summary>
        /// Provides the base encryption process
        /// </summary>
        protected CryptoManagerBase(IKeyProvider provider, SymmetricAlgorithm algorithm)
        {
            _provider = provider;
            _algorithm = algorithm;
        }

        public virtual ReadOnlyMemory<byte> Encrypt(ReadOnlyMemory<byte> buffer)
        {
            return EncryptAsync(buffer, CancellationToken.None).Result;
        }

        public virtual ReadOnlyMemory<byte> Decrypt(ReadOnlyMemory<byte> buffer)
        {
            return DecryptAsync(buffer, CancellationToken.None).Result;
        }

        public async Task<ReadOnlyMemory<byte>> EncryptAsync(ReadOnlyMemory<byte> buffer, CancellationToken token)
        {
            if (buffer.Length == 0)
                throw new ArgumentException(nameof(buffer) + "cannot be empty");

            return await EncryptAsyncInternal(buffer, token);
        }

        private async Task<ReadOnlyMemory<byte>> EncryptAsyncInternal(ReadOnlyMemory<byte> buffer, CancellationToken token)
        {
            var encryptor = _algorithm.CreateEncryptor(_provider.Key, _provider.Vi);
            await using var memoryStream = new MemoryStream();
            await using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            await cryptoStream.WriteAsync(buffer, token);

            cryptoStream.Close();

            return await ReadResultFromStream(memoryStream, buffer.Length);
        }

        public async Task<ReadOnlyMemory<byte>> DecryptAsync(ReadOnlyMemory<byte> buffer, CancellationToken token)
        {
            var decryptor = _algorithm.CreateDecryptor(_provider.Key, _provider.Vi);
            await using var memoryStream = new MemoryStream(buffer.ToArray());
            await using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            var buf = new byte[buffer.Length];

            await cryptoStream.ReadAsync(buf, 0, buf.Length, token);
            await cryptoStream.FlushAsync(token);

            cryptoStream.Close();

            return new Memory<byte>(buf);
        }

        private static async Task<ReadOnlyMemory<byte>> ReadResultFromStream(MemoryStream stream,
            int bufferLen) =>
            new Memory<byte>(stream.ToArray());
    }
}