using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CodeServiceJD.BackgroundQ
{
    public class BackgroundQService<T> : BackgroundService
    {
        private readonly ILogger<BackgroundQService<T>> _logger;

        private readonly IBackgroundQ<T> _queue;
        private readonly IBackgroundQProcessor<T> _processor;

        public BackgroundQService(IBackgroundQ<T> queue, IBackgroundQProcessor<T> processor, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BackgroundQService<T>>();
            _queue = queue;
            _processor = processor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"Background Q for {typeof(T).Name} ExecuteAsync is starting.");

            stoppingToken.Register(() =>
                    _logger.LogDebug($"Background Q for {typeof(T).Name} background task cancellation of stoppingToken."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug($"Background Q for {typeof(T).Name} task doing background work.");

                T element = await _queue.GetElementAsync(stoppingToken);

                try
                {
                    await _processor.ProcessQElementAsync(element);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Background Q for {typeof(T).Name} internal exception");
                }
            }

            _logger.LogDebug($"Background Q for {typeof(T).Name} background task ExecuteAsync finished.");
        }
    }
}
