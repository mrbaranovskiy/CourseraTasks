namespace Storeroom
{
    /// <summary>
    /// Represents position the chunk of data.
    /// </summary>
    public interface IChunk
    {
        IRecord Reference { get; }
        InBlobPosition Position { get; }
    }
}