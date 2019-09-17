using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Indicators;
using core.Trading.Indicators.Options;
using core.TypeCodes;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class TemaExtension
	{
		public static List<decimal?> Tema (this IEnumerable<ICandle> candles, int? period = null, ICandleVariableCode type = null)
		{
			period ??= 30;
			type ??= CandleVariableCode.CLOSE;

			IIndicatorOptions options = new TemaOptions(period.Value, type);
			Tema tema = new Tema();
			return tema.Get(candles, options);
		}
	}
}
