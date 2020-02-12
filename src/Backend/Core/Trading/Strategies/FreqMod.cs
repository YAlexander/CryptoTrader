using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;


namespace Core.Trading.Strategies
{
	public class FreqMod : BaseStrategy
	{
		public override string Name { get; } = "Freq Modded";
		
		public override int MinNumberOfCandles { get; } = 100;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> sma = candles.Sma(100);
			List<decimal> closes = candles.Select(x => x.Close).ToList();
			List<decimal?> adx = candles.Adx(14);
			List<decimal?> mfi = candles.Mfi(14);
			List<decimal?> cci = candles.Cci(5);
			List<decimal?> rsi = candles.Rsi();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (closes[i] < sma[i] && cci[i] < -100 && adx[i] > 20 && mfi[i] < 30 && rsi[i] < 25)
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (cci[i] > 100 && mfi[i] > 80 && rsi[i] > 70)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
			}

			return result;
		}
	}
}