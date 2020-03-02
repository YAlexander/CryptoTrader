using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Enums;
using Persistence.Entities;

namespace Persistence
{
	public interface ICandlesProcessor
	{
		Task<IEnumerable<Candle>> GetCandles(Exchanges exchange, Assets asset1, Assets asset2, DateTime from, DateTime to);

		Task<IEnumerable<Candle>> GetCandles(Exchanges exchange, Assets asset1, Assets asset2, int numberOfLastCandles);

		Task<long> Create (Candle candle);
	}
}