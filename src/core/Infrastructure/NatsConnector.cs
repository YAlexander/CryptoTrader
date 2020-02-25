using Microsoft.Extensions.Options;
using MyNatsClient;

namespace core.Infrastructure
{
	public class NatsConnector
	{
		private IOptions<AppSettings> _settings;
		private NatsClient _client;

		public NatsClient Client 
		{ get {
				if (!_client.IsConnected)
				{
					_client.Connect();
				}

				return _client;
			}
		}

		public NatsConnector(IOptions<AppSettings> settings)
		{
			ConnectionInfo cnInfo = new ConnectionInfo(_settings.Value.BusConnectionString);
			_client = new NatsClient(cnInfo);
		}
	}
}
