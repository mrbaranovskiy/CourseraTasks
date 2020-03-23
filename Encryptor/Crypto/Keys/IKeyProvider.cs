using System;
using System.Runtime.CompilerServices;
using System.Security;

[assembly:InternalsVisibleTo("Coursera.EncryptorTest")]

namespace Encryptor.Crypto.Keys
{
    /// <summary>
    /// Base key provider
    /// </summary>
    public interface IKeyProvider
    {
        byte[] Key { get; }
        byte[] IV { get; }
    }
}