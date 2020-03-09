using System;
using System.Runtime.InteropServices.ComTypes;

namespace Storeroom
{
    public interface IFileSystem
    {
        IStream Read(string file);
        bool Write(string file);
        bool Write(Span<byte> data);
        bool Write(byte[] data);
        
        bool Remove(string file);

        /// <summary> Goes through the journal and remove unreferenced files. </summary>
        void Gc();
    }
    
    public class FileSystem : IFileSystem
    {
        public FileSystem(ICredManager manager, IEncryptor crypto)
        {
            
        }
        
        public IStream Read(string file)
        {
            throw new NotImplementedException();
        }

        public bool Write(string file)
        {
            throw new NotImplementedException();
        }

        public bool Write(Span<byte> data)
        {
            throw new NotImplementedException();
        }

        public bool Write(byte[] data)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string file)
        {
            throw new NotImplementedException();
        }

        public void Gc()
        {
            throw new NotImplementedException();
        }
    }
}