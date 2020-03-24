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
		public async Task<IEnumerable<IExchangeSettings>> Get(Exchanges exchange)
		{
			return await WithConnection<IEnumerable<IExchangeSettings>>((connection, transaction) => _settingsManager.Get(exchange, connection, transaction));
		}

		public Task<IExchangeSettings> Get(Exchanges exchange, Assets asset1, Assets asset2)
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