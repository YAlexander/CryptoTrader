using core.Abstractions.Database;
using core.Abstractions.TypeCodes;
using core.Infrastructure.Database.Entities;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using core.Infrastructure.Database.Managers;
using Microsoft.Extensions.Logging;

namespace core.Infrastructure.BL
{
	public class ExchangeConfigProcessor : BaseProcessor
	{
		private IExchangeConfigManager _exchangeConfigManager;
		private IPairConfigManager _pairConfigManager;

		public ExchangeConfigProcessor (
			ILogger<ExchangeConfigProcessor> logger,
			IOptions<AppSettings> settings,
			IExchangeConfigManager exchangeConfigManager,
			IPairConfigManager pairConfigManager
			) : base(settings, logger)
		{
			_exchangeConfigManager = exchangeConfigManager;
			_pairConfigManager = pairConfigManager;
		}

		public async Task<ExchangeConfig> GetExchangeConfig(int exchangeCode)
		{
			return await WithConnection(async (connection, transaction) =>
			{
				ExchangeConfig exchangeConfig = await _exchangeConfigManager.Get(exchangeCode, connection, transaction);
				if (exchangeConfig != null)
				{
					exchangeConfig.Pairs = (List<PairConfig>)await _pairConfigManager.GetAssignedPairs(exchangeCode, connection, transaction);
				}

				return exchangeConfig;
			});
		}

		public Task<IEnumerable<ExchangeConfig>> GetExchangeConfigs ()
		{
			return WithConnection((connection, transaction) =>
			{
				return _exchangeConfigManager.GetAll(connection, transaction);
			});
		}
	}
}
