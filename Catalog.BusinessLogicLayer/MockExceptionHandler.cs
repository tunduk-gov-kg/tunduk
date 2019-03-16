using System;
using NLog;

namespace Catalog.BusinessLogicLayer
{
    public class MockExceptionHandler : IExceptionHandler
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static MockExceptionHandler()
        {
        }

        public void Handle(Exception exception)
        {
            _logger.Error(exception);
        }
    }
}