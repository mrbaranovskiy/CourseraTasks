using System.Collections.Generic;

namespace Storeroom
{
    /// <summary> Defines the record </summary>
    public interface IRecord
    {
        IRecordIdentifier Identifier { get; }
        IEnumerable<IChunk> Chunks { get; }
    }
}