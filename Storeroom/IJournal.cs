using System.Collections.Generic;

namespace Storeroom
{
    /// <summary> Journal </summary>
    interface IJournal
    {
        IRecord FindRecord(IRecordIdentifier identifier);
        bool RemoveRecord(IRecordIdentifier identifier);
    }
}