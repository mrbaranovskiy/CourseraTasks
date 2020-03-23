using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace Encryptor.Crypto.Keys
{
    internal class SimpleKeyProvider : IKeyProvider
    {
        private byte[] _key;
        private byte[] _iv;

        public SimpleKeyProvider(string secret, SymmetricAlgorithm algo)
        {
            var size = algo.KeySize / 8;
            var blockSize = algo.BlockSize / 8;
            var byteArr = Encoding.Unicode.GetBytes(secret);
            //Sha1 is not enough
            var secretHash = HashingUtils.ComputeHash(byteArr, HashAlgorithmType.Sha1);
            _key = new byte[size];
            _iv = new byte[blockSize];

            for (int i = 0; i < size; i++)
            {
                var idx = i % secretHash.Length;
                _key[i] = secretHash[idx];
            }

            for (int i = 0; i < blockSize; i++)
            {
                var idx = i % secretHash.Length;
                _iv[i] = secretHash[idx];
            }
        }

        public byte[] Key => _key;
        public byte[] IV => _iv;
    }
}