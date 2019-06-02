using System;
using System.Collections.Generic;
using Monitor.Domain.Entity;

namespace Monitor.Domain.Repository
{
    public interface IOpDataRepository
    {
        void InsertRecords(OpDataRecord[] opDataRecords);
        IList<OpDataRecord> GetOpDataRecordsBatch(int batchSize, Predicate<OpDataRecord> specification);
    }
}