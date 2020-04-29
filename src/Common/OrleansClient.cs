using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Silo;

namespace Common
{
	public class OrleansClient
	{
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

		public OrleansClient(IOptions<AppSettings> settings)
		{
			_settings = settings;
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
						.AddSimpleMessageStreamProvider(Constants.MessageStreamProvider)
				.Build();
		}
	}
}
