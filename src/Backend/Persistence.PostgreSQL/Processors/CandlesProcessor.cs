using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using Contracts.Enums;
using Persistence.Entities;
using Persistence.Managers;

namespace Persistence.PostgreSQL.Processors
{
	public class CandlesProcessor : BaseProcessor, ICandlesProcessor
	{
		private readonly ICandlesManager _candlesManager;
		
		public CandlesProcessor(string connectionString, ICandlesManager candlesManager) : base(connectionString)
		{
			_candlesManager = candlesManager;
		}

		public Task<IEnumerable<ICandle>> GetCandles(Exchanges exchange, Assets asset1, Assets asset2, DateTime from, DateTime to)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<ICandle>> GetCandles(Exchanges exchange, Assets asset1, Assets asset2, int numberOfCandles)
		{
			return await WithConnection((connection, transaction) => _candlesManager.Get(exchange, asset1, asset2, numberOfCandles, connection, transaction));
		}

		public async Task<ICandle> Create(Candle candle)
		{
			return await WithConnection((connection, transaction) => _candlesManager.Create(candle, connection, transaction));
		}

		public Task<ICandle> Find(Exchanges exchange, Assets asset1, Assets asset2, DateTime time)
		{
			throw new NotImplementedException();
		}
	}
}