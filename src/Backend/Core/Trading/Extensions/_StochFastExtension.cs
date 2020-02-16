using Core.Trading.Indicators.Options;
using System.Collections.Generic;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators;
using Core.Trading.Models;

namespace Core.Trading.Extensions
{
	public static class StochFastExtension
	{
		public static StochItem StochFast (this IEnumerable<ICandle> candles, int? fastKPeriod = null, int? fastDPeriod = null, TicTacTec.TA.Library.Core.MAType? fastDmaType = null)
		{
			fastKPeriod ??= 5;
			fastDPeriod ??= 3;
			fastDmaType ??= TicTacTec.TA.Library.Core.MAType.Sma;

			IIndicatorOptions options = new StochFastOptions(fastKPeriod.Value, fastDPeriod.Value, fastDmaType.Value);
			StochFast stochFast = new StochFast();
			return (StochItem)stochFast.Get(candles, options);
		}
	}
}