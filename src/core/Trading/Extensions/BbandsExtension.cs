using core.Abstractions;
using core.Trading.Indicators;
using core.Trading.Indicators.Options;
using core.Trading.Models;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class BbandsExtension
	{
		public static BbandItem Bbands (this IEnumerable<ICandle> candles, int? period = null, double? devUp = null, double? devDown = null, TicTacTec.TA.Library.Core.MAType? type = null)
		{
			period ??= 5;
			devUp ??= 2;
			devDown ??= 2;
			type ??= TicTacTec.TA.Library.Core.MAType.Sma;

			IIndicatorOptions options = new BbandsOptions(period.Value, devUp.Value, devDown.Value, type.Value);
			Bbands bbands = new Bbands();
			return (BbandItem)bbands.Get(candles, options);
		}
	}
}