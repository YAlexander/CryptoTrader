using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;

namespace Common
{
    public class OrleansClient : IHostedService
    {
        private readonly ILogger<OrleansClient> _logger;

        public OrleansClient(ILogger<OrleansClient> logger, ILoggerProvider loggerProvider)
        {
            _logger = logger;
            Client = new ClientBuilder()
                .UseLocalhostClustering()
                .ConfigureLogging(builder => builder.AddProvider(loggerProvider))
                .Build();
        }

        public IClusterClient Client { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            int attempt = 0;
            int maxAttempts = 100;
            TimeSpan delay = TimeSpan.FromSeconds(1);
            
            return Client.Connect(async error =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return false;
                }

                if (++attempt < maxAttempts)
                {
                    _logger.LogWarning(error,
                        "Failed to connect to Orleans cluster on attempt {@Attempt} of {@MaxAttempts}.",
                        attempt, maxAttempts);

                    try
                    {
                        await Task.Delay(delay, cancellationToken);
                    }
                    catch (OperationCanceledException)
                    {
                        return false;
                    }

                    return true;
                }
                else
                {
                    _logger.LogError(error,
                        "Failed to connect to Orleans cluster on attempt {@Attempt} of {@MaxAttempts}.",
                        attempt, maxAttempts);

                    return false;
                }
            });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Client.Close();
            }
            catch (OrleansException error)
            {
                _logger.LogWarning(error, "Error while gracefully disconnecting from Orleans cluster. Will ignore and continue to shutdown.");
            }
        }
    }
}
