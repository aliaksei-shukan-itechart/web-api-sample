using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sample.Impl.Services.ToDoTasks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Impl.HostedServices
{
    public class ToDoTasksChecker : BackgroundService
    {
        public IServiceProvider Services { get; set; }

        private readonly ILogger<ToDoTasksChecker> _logger;

        public ToDoTasksChecker(
            ILogger<ToDoTasksChecker> logger,
            IServiceProvider services)
        {
            _logger = logger;
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Checker Service is running");

            await DoWork(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Checker Service is stopping.");

            await base.StopAsync(stoppingToken);
        }


        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Checker Service is working.");

            using (var scope = Services.CreateScope())
            {
                var tasksService = scope.ServiceProvider
                        .GetRequiredService<ITasksService>();

                await tasksService.CheckExpirationTimeAsync(stoppingToken);
            }
        }
    }
}
