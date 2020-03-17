using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using TechanCore;

namespace Abstractions
{
	// For Grain identity we use Exchange code as primary key and GrainKeyExtension as key extension
	public interface ICandleProcessingGrain : IGrainWithIntegerCompoundKey
	{
		Task<IEnumerable<ICandle>> Get (DateTime from, DateTime to);
		
		Task<IEnumerable<ICandle>> Get (int numberOfLastCandles);
	}
}