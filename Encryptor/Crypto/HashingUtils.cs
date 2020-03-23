using System;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Cryptography;
[assembly:InternalsVisibleTo("Coursera.EncryptorTest")]

namespace Encryptor.Crypto
{
    internal static class HashingUtils
    {
        internal static byte[] ComputeHash(byte[] buffer)
        {
            return HashAlgorithm.Create().ComputeHash(buffer);
        }

        internal static byte[] ComputeHash(byte[] buffer, HashAlgorithmType hashType)
        {
            return HashAlgorithm.Create(FromHashAlgo(hashType)).ComputeHash(buffer);
        }

        private static string FromHashAlgo(HashAlgorithmType type) => type switch
        {
            HashAlgorithmType.Sha1 => "SHA1",
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}