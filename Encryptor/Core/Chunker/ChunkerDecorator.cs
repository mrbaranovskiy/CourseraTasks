using Encryptor.Core.Chunker;
using Encryptor.Crypto.Encryption;

namespace Encryptor.Core
{
    internal abstract class ChunkerDecorator : ChunkerBase
    {
        private readonly ChunkerBase _chunker;

        /// <inheritdoc />
        protected ChunkerDecorator(IHashService hashingSerice, ChunkerBase chunker) 
            : base(hashingSerice)
        {
            _chunker = chunker;
        }
    }
}