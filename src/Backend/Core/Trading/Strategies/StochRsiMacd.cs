using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;
using Core.TypeCodes;

namespace Core.Trading.Strategies
{
	public class StochRsiMacd : BaseStrategy
	{
		public override string Name { get; } = "STOCH RSI MACD";

		public override int MinNumberOfCandles { get; } = 99;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			MacdItem macd = candles.Macd(7, 12, 9);
			StochItem stoch = candles.StochRsi(9, CandleVariableCode.Close, 3, 3, TicTacTec.TA.Library.Core.MAType.Sma);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (macd.Macd[i] - macd.Signal[i] < 0 && stoch.K[i] > 70 && stoch.K[i] < stoch.D[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else if (macd.Macd[i] - macd.Signal[i] > 0 && stoch.K[i] < 30 && stoch.K[i] > stoch.D[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
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
