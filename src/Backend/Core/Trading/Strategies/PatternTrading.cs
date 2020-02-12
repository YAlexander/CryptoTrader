using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.TypeCodes;

namespace Core.Trading.Strategies
{
	public class PatternTrading : BaseStrategy
	{
		public override string Name { get; } = "Pattern Trading";

		public override int MinNumberOfCandles { get; } = 80;

		private readonly List<CandlePatternCode> _bearishPatterns = new List<CandlePatternCode>
															{
																CandlePatternCode.BearishHarami,
																CandlePatternCode.BearishKicker,
																CandlePatternCode.BearishMarubozu,
																CandlePatternCode.BearishEngulfing,
																CandlePatternCode.BearishHangingMan,
																CandlePatternCode.BearishEveningStar,
																CandlePatternCode.BearishDarkCloudCover,
																CandlePatternCode.BearishInvertedHammer
															};

		private readonly List<CandlePatternCode> _bullishPatterns = new List<CandlePatternCode>
															{
																CandlePatternCode.BullishBelt,
																CandlePatternCode.BullishHammer,
																CandlePatternCode.BullishHarami,
																CandlePatternCode.BullishKicker,
																CandlePatternCode.BullishEngulfing,
																CandlePatternCode.BullishMarubozu,
																CandlePatternCode.BullishMorningStar
															};

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<CandlePatternCode> patterns = candles.CandlePatterns();
			List<decimal> close = candles.Close();
			List<decimal> open = candles.Open();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (patterns[i - 1] != null)
				{
					if (_bullishPatterns.Contains(patterns[i - 1]) && open[i] >= close[i - 1])
					{
						result.Add((candles.ElementAt(i), TradingAdvices.BUY));
					}
					else if (_bearishPatterns.Contains(patterns[i - 1]) && close[i - 1] < open[i])
					{
						result.Add((candles.ElementAt(i), TradingAdvices.SELL));
					}
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
