namespace Storeroom
{
    /// <summary> Encrypt manager </summary>
    public interface IEncryptor
    {
        /// <summary> Manager </summary>
        ICredManager CredManager { get; }
    }
}