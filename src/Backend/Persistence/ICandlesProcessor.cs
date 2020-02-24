using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using Contracts.Enums;
using Persistence.Entities;

namespace Persistence
{
	public interface ICandlesProcessor
	{
		Task<IEnumerable<Candle>> GetCandles(Exchanges exchange, Assets asset1, Assets asset2, int numberOfCandles);

		Task<long> Create (Candle candle);

		Task<ICandle> Find(Exchanges exchange, Assets asset1, Assets asset2, DateTime time);
	}
}