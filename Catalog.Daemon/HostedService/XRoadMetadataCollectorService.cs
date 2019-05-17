using System;
using System.Threading;
using System.Threading.Tasks;
using Catalog.BusinessLogicLayer;
using Catalog.BusinessLogicLayer.Service;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Catalog.Daemon.HostedService
{
    public class XRoadMetadataCollectorService : IHostedService, IDisposable
    {
        private readonly XRoadMetadataCollector _collector;
        private readonly object _lockObject = new object();
        private readonly ILogger _logger;
        private Timer _timer;

        public XRoadMetadataCollectorService(
            XRoadMetadataCollector collector
            , ILogger<XRoadMetadataCollector> logger)
        {
            _collector = collector;
            _logger = logger;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Collect, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void Collect(object state)
        {
            lock (_lockObject)
            {
                try
                {
                    _logger.LogInformation("Starting XRoad Metadata Collector Task");
                    var task = Task.Run(async () => { await _collector.RunBatchUpdateTask(); });
                    task.Wait();
                    _logger.LogInformation("XRoad Metadata Collector Task Completed");
                }
                catch (Exception exception)
                {
                    _logger.LogError(LoggingEvents.RunBatchUpdateTask,
                        "XRoad Metadata Collector Task Completed with Error: {message}; {stackTrace}",
                        exception.Message, exception.StackTrace);
                }
            }
        }
    }
}