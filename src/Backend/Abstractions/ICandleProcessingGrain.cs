using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using Contracts.Enums;
using Orleans;

namespace Abstractions
{
	public interface ICandleProcessingGrain : IGrainWithIntegerKey
	{
		Task <long?> Create (Exchanges exchange, Assets asset1, Assets asset2, ICandle candle);

		Task<IEnumerable<ICandle>> Get (Exchanges exchange, Assets asset1, Assets asset2, DateTime from, DateTime to);
		
		Task<IEnumerable<ICandle>> Get (Exchanges exchange, Assets asset1, Assets asset2, int numberOfLastCandles);
	}
}