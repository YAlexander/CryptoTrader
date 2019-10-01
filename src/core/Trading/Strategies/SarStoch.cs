using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.Trading.Models;
using core.Trading.Strategies.Presets;
using core.TypeCodes;
using Core.Trading.Extensions;
using TinyJson;

namespace core.Trading.Strategies
{
	public class SarStoch : BaseStrategy
	{
		public override string Name { get; } = "SAR Stoch";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 15;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			SarStochPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JSONParser.FromJson<SarStochPreset>(Preset);
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			StochItem stoch = candles.Stoch(preset?.Stoch ?? 13);
			StochItem stochFast = candles.StochFast(preset?.StochFast ?? 13);
			List<decimal?> sar = candles.Sar(preset?.SarAccelerationFactor ?? 3);

			List<decimal> highs = candles.Select(x => x.High).ToList();
			List<decimal> lows = candles.Select(x => x.Low).ToList();
			List<decimal> closes = candles.Select(x => x.Close).ToList();
			List<decimal> opens = candles.Select(x => x.Open).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i > 2)
				{
					decimal? currentSar = sar[i];
					decimal? priorSar = sar[i - 1];
					decimal lastHigh = highs[i];
					decimal lastLow = lows[i];
					decimal lastOpen = opens[i];
					decimal lastClose = closes[i];
					decimal priorHigh = highs[i - 1];
					decimal priorLow = lows[i - 1];
					decimal priorOpen = opens[i - 1];
					decimal priorClose = closes[i - 1];
					decimal prevOpen = opens[i - 2];
					decimal prevClose = closes[i - 2];

					bool below = currentSar < lastLow;
					bool above = currentSar > lastHigh;
					bool redCandle = lastOpen < lastClose;
					bool greenCandle = lastOpen > lastClose;
					bool priorBelow = priorSar < priorLow;
					bool priorAbove = priorSar > priorHigh;
					bool priorRedCandle = priorOpen < priorClose;
					bool priorGreenCandle = priorOpen > priorClose;
					bool prevRedCandle = prevOpen < prevClose;
					bool prevGreenCandle = prevOpen > prevClose;

					priorRedCandle = prevRedCandle || priorRedCandle;
					priorGreenCandle = prevGreenCandle || priorGreenCandle;

					int fsar = 0;

					if (priorAbove && priorRedCandle && below && greenCandle)
					{
						fsar = 1;
					}
					else if (priorBelow && priorGreenCandle && above && redCandle)
					{
						fsar = -1;
					}

					if (fsar == -1 && (stoch.K[i] > 90 || stoch.D[i] > 90 || stochFast.K[i] > 90 || stochFast.D[i] > 90))
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
					}
					else if (fsar == 1 && (stoch.K[i] < 10 || stoch.D[i] < 10 || stochFast.K[i] < 10 || stochFast.D[i] < 10))
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
					}
					else
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
					}
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
			}

			return result;
		}
	}
}