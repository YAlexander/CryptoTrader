using core.Abstractions;
using core.Trading.Indicators;
using core.Trading.Models;
using Core.Trading.Indicators.Options;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class StochExtension
	{
		public static StochItem Stoch (this IEnumerable<ICandle> candles, int? fastPeriod = null, int? slowKPeriod = null, TicTacTec.TA.Library.Core.MAType? slowKmaType = null, int? slowDPeriod = null, TicTacTec.TA.Library.Core.MAType? slowDmaType = null)
		{
			fastPeriod ??= 5;
			slowKPeriod ??= 3;
			slowKmaType ??= TicTacTec.TA.Library.Core.MAType.Sma;
			slowDPeriod ??= 3;
			slowDmaType ??= TicTacTec.TA.Library.Core.MAType.Sma;

			IIndicatorOptions options = new StochOptions(fastPeriod.Value, slowDPeriod.Value, slowKmaType.Value, slowDPeriod.Value, slowDmaType.Value);
			Stoch stosh = new Stoch();
			return (StochItem)stosh.Get(candles, options);
		}
	}
}
