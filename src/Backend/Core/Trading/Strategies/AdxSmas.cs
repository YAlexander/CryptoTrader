using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.TAIndicators.Extensions;
using Core.TypeCodes;

namespace Core.Trading.Strategies
{
	public class AdxSmas : BaseStrategy
	{
		public override string Name { get; } = "ADX Smas";

		public override int MinNumberOfCandles { get; } = 14;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> smaShort = candles.Sma(3, CandleVariableCode.Close);
			List<decimal?> smaLong = candles.Sma(10, CandleVariableCode.Close);
			List<decimal?> adx = candles.Adx(14);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else
				{
					int sixCross = smaShort[i - 1] < smaLong[i] && smaShort[i] > smaLong[i] ? 1 : 0;
					int fortyCross = smaLong[i - 1] < smaShort[i] && smaLong[i] > smaShort[i] ? 1 : 0;

					if (adx[i] > 25 && sixCross == 1)
					{
						result.Add((candles[i], TradingAdvices.BUY));
					}
					else if (adx[i] < 25 && fortyCross == 1)
					{
						result.Add((candles[i], TradingAdvices.SELL));
					}
					else
					{
						result.Add((candles[i], TradingAdvices.HOLD));
					}
				}
			}

			return result;
		}
	}
}
