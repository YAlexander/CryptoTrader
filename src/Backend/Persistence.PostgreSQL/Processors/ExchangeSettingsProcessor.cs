using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Enums;
using Common;
using Microsoft.Extensions.Options;
using Persistence.Entities;
using Persistence.Managers;

namespace Persistence.PostgreSQL.Processors
{
	public class ExchangeSettingsProcessor : BaseProcessor, ISettingsProcessor
	{
		public Task<IEnumerable<ExchangeSettings>> Get(Exchanges exchange)
		{
			return WithConnection<IEnumerable<ExchangeSettings>>((connection, transaction) => _settingsManager.Get(exchange, connection, transaction));
		}

		public Task<ExchangeSettings> Get(Exchanges exchange, Assets asset1, Assets asset2)
		{
			throw new System.NotImplementedException();
		}

		private readonly IExchangeSettingsManager _settingsManager;
		
		public ExchangeSettingsProcessor(IOptions<DatabaseOptions> options, IExchangeSettingsManager settingsManager) : base(options.Value.CryptoTradingConnectionString)
		{
			_settingsManager = settingsManager;
		}
	}
}