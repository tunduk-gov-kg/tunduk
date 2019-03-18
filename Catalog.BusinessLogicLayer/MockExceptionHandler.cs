using System;
using NLog;

namespace Catalog.BusinessLogicLayer
{
    public class MockExceptionHandler : IExceptionHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static MockExceptionHandler()
        {
            
        }

        public void Handle(Exception exception)
        {
            Logger.Error(exception);
        }
    }
}