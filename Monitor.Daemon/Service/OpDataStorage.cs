using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Monitor.Domain.Entity;
using Nest;

namespace Monitor.Daemon.Service
{
    public class OpDataStorage : IOpDataStorage
    {
        private readonly Uri _elasticNode;
        private readonly ILogger<OpDataStorage> _logger;

        public OpDataStorage(IConfiguration configuration, ILogger<OpDataStorage> logger)
        {
            _elasticNode = new Uri(configuration["ElasticSearchNode"]);
            _logger = logger;
        }

        public void Store(OpDataRecord[] dataRecords)
        {
            if (dataRecords.Length <= 0) return;

            var connectionSettings = new ConnectionSettings(_elasticNode);
            var elasticClient = new ElasticClient(connectionSettings);

            var bulkResponse = elasticClient.IndexMany(dataRecords, "op_data_records");
            _logger.LogInformation(bulkResponse.DebugInformation);
        }
    }
}