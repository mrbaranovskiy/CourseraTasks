using System;
using System.Security.Cryptography;
using System.Text;
using Encryptor.Crypto;
using Moq;

namespace EncryptorTest
{
    public static class TestUtils
    {
        public static Memory<byte> TextToBuffer(string text)
        {
            var encodedText = Encoding.ASCII.GetBytes(text);
            return new Memory<byte>(encodedText);
        }

        public static string BufferToText(Span<byte> buffer) => Encoding.ASCII.GetString(buffer);

        public static IKeyProvider KeyProvider(SymmetricAlgorithm cryptor)
        {
            cryptor.GenerateKey();
            cryptor.GenerateIV();

            var keyMock = new Mock<IKeyProvider>();
            keyMock.SetupGet(p => p.Key).Returns(cryptor.Key);
            keyMock.SetupGet(p => p.Vi).Returns(cryptor.IV);

            return keyMock.Object;
        }
    }
}