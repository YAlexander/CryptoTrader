using System.Collections.Generic;
using Contracts;
using Contracts.Trading;
using Contracts.TypeCodes;
using Core.Trading.Indicators;
using Core.Trading.Indicators.Options;
using Core.TypeCodes;

namespace Core.Trading.Extensions
{
	public static class TemaExtension
	{
		public static List<decimal?> Tema (this IEnumerable<ICandle> candles, int? period = null, ICandleVariableCode type = null)
		{
			period ??= 30;
			type ??= CandleVariableCode.Close;

			IIndicatorOptions options = new TemaOptions(period.Value, type);
			Tema tema = new Tema();
			return tema.Get(candles, options);
		}
	}
}
