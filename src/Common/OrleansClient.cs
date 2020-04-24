using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;
using Silo;

namespace Common
{
    public class OrleansClient
    {
        private readonly ILogger<OrleansClient> _logger;
        private readonly IOptions<AppSettings> _settings;
        
        private IClusterClient _client;
        public IClusterClient Client
        {
            get
            {
                if (_client == null || !_client.IsInitialized)
                {
                    _client = Init();
                    _client.Connect();
                }

                return _client;
            }
        }

        public OrleansClient(IOptions<AppSettings> settings, ILogger<OrleansClient> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public async Task ConnectAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(30));

            int attempt = 0;
            int maxAttempts = 1000;
            TimeSpan delay = TimeSpan.FromSeconds(3);
            _logger.LogTrace("Connecting to Orleans cluster");

            await Client.Connect(async error =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return false;
                }

                if (++attempt < maxAttempts)
                {
                    _logger.LogWarning(error, "Failed to connect to Orleans cluster on attempt {@Attempt} of {@MaxAttempts}.", attempt, maxAttempts);

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
                    _logger.LogError(error, "Failed to connect to Orleans cluster on attempt {@Attempt} of {@MaxAttempts}.", attempt, maxAttempts);

                    return false;
                }
            });
        }

        private async Task StopAsync(CancellationToken cancellationToken)
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
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = _settings.Value.ClusterId;
                            options.ServiceId = _settings.Value.ServiceId;
                        })
                        .UseAdoNetClustering(options =>
                        { 
                             options.Invariant = _settings.Value.DatabaseOptions.DatabaseProvider;
                             options.ConnectionString = _settings.Value.DatabaseOptions.SystemConnectionString;
                        })
                        //.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(ITradingGrain).Assembly).WithReferences())
                        .AddSimpleMessageStreamProvider(Constants.MessageStreamProvider)
                .Build();
        }

		public void Dispose()
		{
            Client.Close();
            Client.Dispose();
		}
	}
}
