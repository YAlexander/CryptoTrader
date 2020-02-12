using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Enums;
using Persistence.Entities;

namespace Persistence
{
	public class CandlesProcessor : BaseProcessor, ICandlesProcessor
	{
		private readonly ICandlesManager _candlesManager;
		
		public CandlesProcessor(string connectionString, ICandlesManager candlesManager) : base(connectionString)
		{
			_candlesManager = candlesManager;
		}

		public Task<IEnumerable<Candle>> GetCandles(Exchanges exchange, Assets asset1, Assets asset2, DateTime from, DateTime to,  Timeframes period)
		{
			return WithConnection((connection, transaction) => _candlesManager.Get(exchange, asset1, asset2, @from, to, period, connection, transaction));
		}
	}
}