using Core.Trading.Indicators;
using System.Collections.Generic;
using Contracts;
using Contracts.Trading;
using Contracts.TypeCodes;
using Core.Trading.Indicators.Options;
using Core.Trading.Models;
using Core.TypeCodes;

namespace Core.Trading.Extensions
{
	public static class StochRsiExtension
	{
		public static StochItem StochRsi (this IEnumerable<ICandle> candles, int? period = null, ICandleVariableCode type = null, int? fastKPeriod = null, int? fastDPeriod = null, TicTacTec.TA.Library.Core.MAType? fastDmaType = null)
		{
			period ??= 14;
			type ??= CandleVariableCode.Close;
			fastDPeriod ??= 3;
			fastDmaType ??= TicTacTec.TA.Library.Core.MAType.Sma;

			IIndicatorOptions options = new StochRsiOptions(period.Value, type, fastKPeriod.Value, fastDPeriod.Value, fastDmaType.Value);
			StochRsi stochRsi = new StochRsi();
			return (StochItem)stochRsi.Get(candles, options);
		}
	}
}