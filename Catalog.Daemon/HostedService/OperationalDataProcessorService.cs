using System;
using System.Threading;
using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Catalog.Daemon.HostedService
{
    public class OperationalDataProcessorService : IHostedService, IDisposable
    {
        private readonly object _lockObject = new object();

        private readonly ILogger _logger;
        private readonly XRoadOperationalDataProcessor _processor;
        private Timer _timer;

        public OperationalDataProcessorService(ILogger<OperationalDataProcessorService> logger
            , XRoadOperationalDataProcessor processor)
        {
            _logger = logger;
            _processor = processor;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(nameof(OperationalDataProcessorService) + " is starting");

            _timer = new Timer(Process, null, TimeSpan.Zero, TimeSpan.FromSeconds(40));
            
            return Task.CompletedTask;
        }

        private void Process(object state)
        {
            lock (_lockObject)
            {
                _logger.LogInformation("Starting Operational Data Processor Task");
                _processor.ProcessRecords();
                _logger.LogInformation("Operational Data Processor Task Completed");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(nameof(OperationalDataProcessorService) + " is stopping");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}