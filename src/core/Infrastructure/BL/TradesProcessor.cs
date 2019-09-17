using core.Abstractions.Database;
using core.Abstractions.TypeCodes;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Database.Managers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace core.Infrastructure.BL
{
	public class TradesProcessor : BaseProcessor
	{
		private ITradesManager _tradesManager;

		public TradesProcessor(
			ILogger<TradesProcessor> logger,
			IOptions<AppSettings> settings,
			ITradesManager tradesManager) : base(settings, logger)
		{
			_tradesManager = tradesManager;
		}

		public async Task<long?> Create(Trade trade)
		{
			return await WithConnection(async (connection, transaction) =>
			{
				return await _tradesManager.Create(trade, connection, transaction);
			});
		}

		public async Task<Trade> GetLastTrade (IExchangeCode exchangeCode, string symbol)
		{
			return await WithConnection(async (connection, transaction) =>
			{
				return await _tradesManager.GetLast(exchangeCode.Code, symbol, connection, transaction);
			});
		}
	}
}
