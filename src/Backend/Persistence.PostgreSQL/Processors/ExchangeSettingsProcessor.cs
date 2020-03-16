using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Enums;
using Persistence.Entities;
using Persistence.Managers;

namespace Persistence.PostgreSQL.Processors
{
	public class ExchangeSettingsProcessor : BaseProcessor, ISettingsProcessor
	{
		public async Task<IEnumerable<IExchangeSettings>> Get(Exchanges exchange)
		{
			return await WithConnection<IEnumerable<IExchangeSettings>>((connection, transaction) => { return _settingsManager.Get(exchange, connection, transaction); });
		}

		public Task<IExchangeSettings> Get(Exchanges exchange, Assets asset1, Assets asset2)
		{
			throw new System.NotImplementedException();
		}

		private readonly IExchangeSettingsManager _settingsManager;
		
		public ExchangeSettingsProcessor(string connectionString, IExchangeSettingsManager settingsManager) : base(connectionString)
		{
			_settingsManager = settingsManager;
		}
	}
}