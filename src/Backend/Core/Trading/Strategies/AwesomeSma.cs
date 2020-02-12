using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;

namespace Core.Trading.Strategies
{
	public class AwesomeSma : BaseStrategy
	{
		public override string Name { get; } = "Awesome SMA";

		public override int MinNumberOfCandles { get; } = 40;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> ao = candles.AwesomeOscillator();
			List<decimal?> smaShort = candles.Sma(20);
			List<decimal?> smaLong = candles.Sma(40);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (ao[i] > 0 && ao[i - 1] < 0 && smaShort[i] > smaLong[i] || ao[i] > 0 && smaShort[i] > smaLong[i] && smaShort[i - 1] < smaLong[i - 1])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (smaShort[i] < smaLong[i] && smaShort[i - 1] > smaLong[i - 1])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
			}

			return result;
		}
	}
}