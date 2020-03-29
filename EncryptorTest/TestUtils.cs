using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Encryptor.Crypto;
using Encryptor.Crypto.Keys;
using Moq;

namespace EncryptorTest
{
    public static class TestUtils
    {
        
        private static Random random = new Random();
        public static Memory<byte> TextToBuffer(string text)
        {
            var encodedText = Encoding.ASCII.GetBytes(text);
            return new Memory<byte>(encodedText);
        }

        public static Stream StreamOfLength(int stringLen, Encoding encoding, out string genString)
        {
            var randomString = RandomString(stringLen);
            var bytes = encoding.GetBytes(randomString);

            genString = randomString;
            return new MemoryStream(bytes, false);
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string BufferToText(Span<byte> buffer) => Encoding.ASCII.GetString(buffer);

        public static IKeyProvider KeyProvider(SymmetricAlgorithm cryptor)
        {
            cryptor.GenerateKey();
            cryptor.GenerateIV();

            var keyMock = new Mock<IKeyProvider>();
            keyMock.SetupGet(p => p.Key).Returns(cryptor.Key);
            keyMock.SetupGet(p => p.IV).Returns(cryptor.IV);

            return keyMock.Object;
        }

        public static bool CompareBuffers(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i])
                    return false;

            return true;
        }
    }
}