using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography;

namespace Encryptor
{
    //todo: implements chunker
    //todo: implement encryption
    class Program
    {
        static void Main(string[] args)
        {
            string secret = "D";

            RijndaelManaged rmCrypto = new RijndaelManaged();
            rmCrypto.GenerateKey();
            rmCrypto.GenerateIV();

            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms,rmCrypto.CreateEncryptor(), CryptoStreamMode.Write);
            using var writer = new StreamWriter(cs);
            writer.Write(secret);
            writer.Close();

            var encrypted = ms.ToArray();

            using var dectypted = new MemoryStream(encrypted);
            using var dectyptor = new CryptoStream(dectypted, rmCrypto.CreateDecryptor(rmCrypto.Key, rmCrypto.IV), CryptoStreamMode.Read);
            using var reader = new StreamReader(dectyptor);

            var readToEnd = reader.ReadToEnd();
        }
    }
}