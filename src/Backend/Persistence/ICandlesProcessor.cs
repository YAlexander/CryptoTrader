using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Enums;
using Persistence.Entities;
using TechanCore;

namespace Persistence
{
	public interface ICandlesProcessor
	{
		Task<ICandle> GetCandle(Exchanges exchange, Assets asset1, Assets asset2, DateTime time);

		Task<IEnumerable<ICandle>> GetCandles(Exchanges exchange, Assets asset1, Assets asset2, DateTime from, DateTime to);

		Task<IEnumerable<ICandle>> GetCandles(Exchanges exchange, Assets asset1, Assets asset2, int numberOfLastCandles);

		Task<ICandle> Create (Candle candle);
	}
}