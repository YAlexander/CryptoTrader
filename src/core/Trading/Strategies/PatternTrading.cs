using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Extensions;
using core.Trading.Extensions;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class PatternTrading : BaseStrategy
	{
		public override string Name { get; } = "Pattern Trading";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 80;

		private List<CandlePatternCode> _bearishPatterns = new List<CandlePatternCode>
															{
																CandlePatternCode.BEARISH_HARAMI,
																CandlePatternCode.BEARISH_KICKER,
																CandlePatternCode.BEARISH_MARUBOZU,
																CandlePatternCode.BEARISH_ENGULFING,
																CandlePatternCode.BEARISH_HANGING_MAN,
																CandlePatternCode.BEARISH_EVENING_STAR,
																CandlePatternCode.BEARISH_DARK_CLOUD_COVER,
																CandlePatternCode.BEARISH_INVERTED_HAMMER
															};

		private List<CandlePatternCode> _bullishPatterns = new List<CandlePatternCode>
															{
																CandlePatternCode.BULLISH_BELT,
																CandlePatternCode.BULLISH_HAMMER,
																CandlePatternCode.BULLISH_HARAMI,
																CandlePatternCode.BULLISH_KICKER,
																CandlePatternCode.BULLISH_ENGULFING,
																CandlePatternCode.BULLISH_MARUBOZU,
																CandlePatternCode.BULLISH_MORNING_STAR
															};

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<CandlePatternCode> patterns = candles.CandlePatterns();
			List<decimal> close = candles.Close();
			List<decimal> open = candles.Open();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else if (patterns[i - 1] != null)
				{
					if (_bullishPatterns.Contains(patterns[i - 1]) && open[i] >= close[i - 1])
					{
						result.Add(TradingAdviceCode.BUY);
					}
					else if (_bearishPatterns.Contains(patterns[i - 1]) && close[i - 1] < open[i])
					{
						result.Add(TradingAdviceCode.SELL);
					}
				}
				else
				{
					result.Add(TradingAdviceCode.HOLD);
				}
			}

			return result.LastOrDefault();
		}
	}
}
