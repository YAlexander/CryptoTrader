using System;
using Contracts;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
	public class AverageDirectionalIndexIndicator : BaseIndicator<AdxOptions, AdxResult>
	{
		public override string Name { get; } = "Average Directional Index (ADX) Indicator";
		
		public override AdxResult Get(ICandle[] source, AdxOptions options)
		{
			decimal?[] atr = source.Atr(options.Period).Result;

			decimal?[] mPlus = source.Momentum(1, CandleVariables.HIGH).Result;
			decimal?[] mMinus = source.Momentum(1, CandleVariables.LOW).Result;
			
			decimal?[] dmPlus = new decimal?[source.Length];
			decimal?[] dmMinus = new decimal?[source.Length];

			for (int i = 0; i < source.Length; i++)
			{
				if (!mPlus[i].HasValue || !mMinus[i].HasValue || !atr[i].HasValue)
				{
					dmPlus[i] = 0;
					dmMinus[i] = 0;
				}
				else
				{
					dmPlus[i] = mPlus[i] > mMinus[i] && mPlus[i] > 0 ? mPlus[i] / atr[i] : 0;
					dmMinus[i] = mMinus[i] > mPlus[i] && mMinus[i] > 0 ? mMinus[i] / atr[i] : 0;
				}
			}

			decimal?[] diPlus = dmPlus.Ema(options.Period).Result;
			decimal?[] diMinus = dmMinus.Ema(options.Period).Result;
			
			decimal?[] dx = new decimal?[source.Length];

			for (int i = 0; i < source.Length; i++)
			{
				if (diPlus[i].HasValue &&!diMinus[i].HasValue)
				{
					dx[i] = Math.Abs(diPlus[i].Value - diMinus[i].Value) / (diPlus[i].Value - diMinus[i].Value);
				}
			}

			decimal?[] adx = dx.Ema(options.Period).Result;
			
			return new AdxResult { Adx = adx, PlusDi = diPlus, MinusDi = diMinus };
		}

		public override AdxResult Get(decimal[] source, AdxOptions options)
		{
			throw new NotImplementedException();
		}

		public override AdxResult Get(decimal?[] source, AdxOptions options)
		{
			throw new NotImplementedException();
		}
	}
}