using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using Contracts.Enums;
using Persistence.Entities;

namespace Persistence.PostgreSQL
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

		public Task<IEnumerable<ICandle>> GetCandles(Exchanges exchange, Assets asset1, Assets asset2, int numberOfCandles)
		{
			throw new NotImplementedException();
		}

		public Task<long> Create(Candle candle)
		{
			throw new NotImplementedException();
		}

		public Task<ICandle> Find(Exchanges exchange, Assets asset1, Assets asset2, DateTime time)
		{
			throw new NotImplementedException();
		}
	}
}