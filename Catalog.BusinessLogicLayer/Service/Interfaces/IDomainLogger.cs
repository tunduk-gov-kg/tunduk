using System;
using Catalog.Domain.Enum;

namespace Catalog.BusinessLogicLayer.Service.Interfaces
{
    public interface IDomainLogger : IDisposable
    {
        void Log(LogLevel logLevel, string message, string description);
    }
}