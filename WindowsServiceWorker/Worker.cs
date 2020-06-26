using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using WindowsServiceWorker.Models;
using WindowsServiceWorker.Service;

namespace WindowsServiceWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly BasicConfiguration _basicConfiguration;
        private readonly IWorkerService _workerService;

        public Worker(ILogger<Worker> logger, BasicConfiguration basicConfiguration, IWorkerService workerService)
        {
            _logger = logger;
            _basicConfiguration = basicConfiguration;
            _workerService = workerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Worker Start {_basicConfiguration.Name} {_workerService.GetName()}");
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
