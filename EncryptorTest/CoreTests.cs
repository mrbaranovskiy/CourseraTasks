using System;
using System.Security.Cryptography;
using System.Text;
using Encryptor.Crypto;
using Encryptor.Crypto.Encryption;
using Encryptor.Crypto.Keys;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EncryptorTest
{
    [TestClass]
    public class KeyProviderTests
    {
        [TestMethod]
        public void TestSimpleKeysProvider()
        {
            const string secret = "Dmytro";
            var cryptor = new RijndaelManaged();
            var simpleKeys = new SimpleKeyProvider(secret, cryptor);

        }
    }

    [TestClass]
    public class CryptoProvideTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var cryptor = new RijndaelManaged();
            var keys = KeyProvider(cryptor);
            var crypto = new ChunkEncryptor(keys, cryptor);

            var textToEncrypt = "Dmytro11";
            var encodedText = Encoding.ASCII.GetBytes(textToEncrypt);
            var buffer = new Memory<byte>(encodedText);
            var readOnlyMemory = crypto.Encrypt(buffer);
            var decrypt = crypto.Decrypt(readOnlyMemory,  textToEncrypt.Length);

            var result = Encoding.ASCII.GetString(decrypt.Span);

            Assert.IsTrue(string.Equals(result, textToEncrypt, StringComparison.Ordinal));
        }

        [TestMethod]
        public void TestEncryptionWithSimplekey()
        {
            string secret = "Password";
            var cryptor = new RijndaelManaged();
            var keys = new SimpleKeyProvider(secret, cryptor);
            var crypto = new ChunkEncryptor(keys, cryptor);

            var data = TestUtils.TextToBuffer("SampleText");
            Assert.IsTrue(EnsureEncryption(data.ToArray(), crypto, keys));

        }

        private bool EnsureEncryption(byte[] inputData, ICrypto crypto, IKeyProvider secret)
        {
            var buffer = new Memory<byte>(inputData);
            var encrypted = crypto.Encrypt(buffer);

            var decrypted = crypto.Decrypt(encrypted);
            return TestUtils.CompareBuffers(decrypted.Slice(0, inputData.Length).ToArray(), inputData);
        }

        private IKeyProvider KeyProvider(SymmetricAlgorithm cryptor)
        {
            cryptor.GenerateKey();
            cryptor.GenerateIV();

            var keyMock = new Mock<IKeyProvider>();
            keyMock.SetupGet(p => p.Key).Returns(cryptor.Key);
            keyMock.SetupGet(p => p.IV).Returns(cryptor.IV);

            return keyMock.Object;
        }
    }
}