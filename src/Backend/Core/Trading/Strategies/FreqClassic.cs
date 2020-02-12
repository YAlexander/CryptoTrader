using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;

namespace Core.Trading.Strategies
{
	public class FreqClassic : BaseStrategy
	{
		public override string Name { get; } = "Freq Classic";
		
		public override int MinNumberOfCandles { get; } = 100;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> sma = candles.Sma(100);
			List<decimal> closes = candles.Close();
			List<decimal?> adx = candles.Adx(14);
			List<decimal?> tema = candles.Tema(4);
			List<decimal?> mfi = candles.Mfi(14);

			List<decimal?> cci = candles.Cci(5);
			List<decimal?> middleBands = candles.Bbands().MiddleBand;

			List<decimal?> fishers = candles.Fisher();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (closes[i] < sma[i] && cci[i] < -100 && fishers[i] < 0 && adx[i] > 20 && mfi[i] < 30 && tema[i] <= middleBands[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (fishers[i] == 1)
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