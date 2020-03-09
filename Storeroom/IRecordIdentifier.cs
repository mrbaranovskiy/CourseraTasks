using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

namespace Storeroom
{
    /// <summary>
    /// Represents the record identifier
    /// </summary>
    public interface IRecordIdentifier 
        : IEqualityComparer<IRecordIdentifier>,
            IComparer<IRecordIdentifier>, IComparer, IComparable<IRecordIdentifier>
    {
        long Id { get; }
        HashCode Hash { get; }
    }
}