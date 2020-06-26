using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
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

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _workerService.Start();
            return base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _workerService.Stop();
            await base.StopAsync(stoppingToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                await BackgroundProcessing(cancellationToken);
            }
            catch (Exception exp)
            {
                _logger.LogError(0, exp, $"Service failed." + Environment.NewLine + exp.Message);
            }
        }

        private async Task BackgroundProcessing(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(_basicConfiguration.Delay), cancellationToken);
            }
        }
    }
}
