using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using Orleans;

namespace Abstractions
{
	// For Grain identity we use Exchange code as primary key and GrainKeyExtension as key extension
	public interface ICandleProcessingGrain : IGrainWithIntegerCompoundKey
	{
		Task <long?> Create (ICandle candle);

		Task<IEnumerable<ICandle>> Get (DateTime from, DateTime to);
		
		Task<IEnumerable<ICandle>> Get (int numberOfLastCandles);
	}
}