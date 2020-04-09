using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Enums;
using Common;
using Microsoft.Extensions.Options;
using Persistence.Entities;
using Persistence.Managers;
using TechanCore;

namespace Persistence.PostgreSQL.Processors
{
	public class CandlesProcessor : BaseProcessor, ICandlesProcessor
	{
		private readonly ICandlesManager _candlesManager;
		
		public CandlesProcessor(IOptions<DatabaseOptions> databaseOptions, ICandlesManager candlesManager) : base(databaseOptions.Value.CryptoTradingConnectionString)
		{
			_candlesManager = candlesManager;
		}

		public async Task<ICandle> GetCandle(Exchanges exchange, Assets asset1, Assets asset2, DateTime time)
		{
			return await WithConnection((connection, transaction) => _candlesManager.Get(exchange, asset1, asset2, time, connection, transaction));
		}

		public async Task<IEnumerable<ICandle>> GetCandles(Exchanges exchange, Assets asset1, Assets asset2, DateTime from, DateTime to)
		{
			return await WithConnection((connection, transaction) => _candlesManager.Get(exchange, asset1, asset2, from, to, connection, transaction));
		}

		public async Task<IEnumerable<ICandle>> GetCandles(Exchanges exchange, Assets asset1, Assets asset2, int numberOfCandles)
		{
			return await WithConnection((connection, transaction) => _candlesManager.Get(exchange, asset1, asset2, numberOfCandles, connection, transaction));
		}

		public async Task<ICandle> Create(Candle candle)
		{
			return await WithConnection((connection, transaction) => _candlesManager.Create(candle, connection, transaction));
		}
	}
}