using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Encryptor.Crypto;
using Encryptor.Crypto.Encryption;

namespace Encryptor.Core.Chunker
{
    internal class ChunkerEncrypted : ChunkerDecorator
    {
        private readonly ICrypto _cryptoServices;

        //DI needed
        internal ChunkerEncrypted(ChunkerBase chunker, ICrypto cryptoServices, IHashService service) 
            : base(service, chunker)
        {
            _cryptoServices = cryptoServices;
        }

        internal override IEnumerable<Chunk> BuildChunks(Stream stream, CancellationToken token)
        {
            //todo: not super efficent bacause we are doing double work
            var chunks = base.BuildChunks(stream, token);

            foreach (var chunk in chunks)
            {
                var mem = new Memory<byte>(chunk.Blob);
                var blobEncrypted = _cryptoServices.Encrypt(mem);
                yield return new Chunk(blobEncrypted.ToArray(), chunk.Hash, chunk.DataLen);
            }
        }
    }
}