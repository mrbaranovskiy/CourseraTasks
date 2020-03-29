using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;
using System.Threading;
using Encryptor.Crypto;
using Encryptor.Crypto.Encryption;

namespace Encryptor.Core.Chunker
{
    internal abstract class ChunkerBase
    {
        protected ChunkerBase(IHashService service)
        {

        }

        internal virtual IEnumerable<Chunk> BuildChunks(Stream stream, CancellationToken token)
        {
            var blockSize = CoreConstants.Chunk512Size;
            stream.Position = 0;

            var pool = ArrayPool<byte>.Create(blockSize,1);
            var total = 0;
            var chunks = new List<Chunk>();
            
            // мне здесь не нравится всё. 
            // Нет управление как мы создаем куски.
            // чанк должен создаваться через базовый класс. 
            while (stream.Length != total)
            {
                try
                {
                    var arr = pool.Rent(blockSize);

                    int bytesLeft = (int)stream.Length - total;
                    int toRead = bytesLeft > blockSize ? blockSize : bytesLeft;
                    var readed = stream.Read(arr, 0, toRead);

                    total += readed;

                    var buffer = new byte[readed];
                    Array.Copy(arr, buffer, readed);
                    var hash = HashingUtils.ComputeHash(arr, HashAlgorithmType.Sha1);
                
                    chunks.Add(new Chunk(buffer, hash, buffer.Length));

                    pool.Return(arr, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
               
            }

            return chunks;
        }
    }
    
     internal class Chunker : ChunkerBase
     {
         internal Chunker(IHashService service) : base(service)
         {
         }

         internal override IEnumerable<Chunk> BuildChunks(Stream stream, CancellationToken token)
         {
             return base.BuildChunks(stream, token);
         }
     }
}