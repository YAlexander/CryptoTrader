using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Indicators.Options;
using core.Trading.Models;
using core.TypeCodes;
using Core.Trading.Indicators;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class StochRsiExtension
	{
		public static StochItem StochRsi (this IEnumerable<ICandle> candles, int? period = null, ICandleVariableCode type = null, int? fastKPeriod = null, int? fastDPeriod = null, TicTacTec.TA.Library.Core.MAType? fastDmaType = null)
		{
			period ??= 14;
			type ??= CandleVariableCode.CLOSE;
			fastDPeriod ??= 3;
			fastDmaType ??= TicTacTec.TA.Library.Core.MAType.Sma;

			IIndicatorOptions options = new StochRsiOptions(period.Value, type, fastKPeriod.Value, fastDPeriod.Value, fastDmaType.Value);
			StochRsi stochRsi = new StochRsi();
			return (StochItem)stochRsi.Get(candles, options);
		}
	}
}