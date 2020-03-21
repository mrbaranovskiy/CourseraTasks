using System;
using System.Runtime.CompilerServices;
using System.Security;
[assembly:InternalsVisibleTo("Coursera.EncryptorTest")]

namespace Encryptor.Crypto
{
    /// <summary>
    /// Base key provider
    /// </summary>
    public interface IKeyProvider
    {
        byte[] Key { get; }
        byte[] Vi { get; }
    }

    internal class KeyProvider : IKeyProvider
    {
        public KeyProvider(SecureString secret)
        {
            throw new NotImplementedException();
        }

        public byte[] Key { get; }
        public byte[] Vi { get; }
    }
}