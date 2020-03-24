using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Runtime;

namespace Common
{
    public class OrleansClient : IHostedService
    {
        private readonly ILogger<OrleansClient> _logger;
        private readonly ILoggerProvider _loggerProvider;
        private IOptions<DatabaseOptions> _settings;
        
        private IClusterClient _client;
        public IClusterClient Client
        {
            get
            {
                if (_client == null || !_client.IsInitialized)
                {
                    _client = Init();
                }

                return _client;
            }
        }

        public OrleansClient(IOptions<DatabaseOptions> settings, ILogger<OrleansClient> logger, ILoggerProvider loggerProvider)
        {
            _logger = logger;
            _loggerProvider = loggerProvider;
            _settings = settings;
        }

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
        
        
        private IClusterClient Init()
        {
            return new ClientBuilder()
                .UseLocalhostClustering()
                .ConfigureLogging(builder => builder.AddProvider(_loggerProvider))
                .Build();
        }
    }
}
