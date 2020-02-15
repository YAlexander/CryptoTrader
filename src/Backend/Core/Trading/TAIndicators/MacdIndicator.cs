using Contracts;
using Contracts.Enums;
using Core.Trading.TAIndicators.Extensions;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators
{
	public class MacdIndicator : BaseIndicator<MacdOptions, MacdIndicatorResult>
	{
		public override string Name { get; } = "Moving Average Convergence Divergence (MACD) Indicator";
		
		public override MacdIndicatorResult Get(ICandle[] source, MacdOptions options)
		{
			decimal?[] macd = new decimal?[source.Length];
			decimal?[] hist = new decimal?[source.Length];
			
			decimal?[] fastEma = source.Ema(options.FastPeriod, CandleVariables.CLOSE).Result;
			decimal?[] slowEma = source.Ema(options.SlowPeriod, CandleVariables.CLOSE).Result;

			for (int i = 0; i < source.Length; i++)
			{
				macd[i] = (fastEma[i] - slowEma[i]) ?? 0;
			}

			decimal?[] signal = macd.Ema(options.SignalPeriod).Result;

			for (int i = 0; i < source.Length; i++)
			{
				hist[i] = macd[i] - signal[i];
			}
			
			return new MacdIndicatorResult() { Macd = macd, Signal = signal, Hist = hist};
		}
		
		public override MacdIndicatorResult Get(decimal[] source, MacdOptions options)
		{
			throw new System.NotImplementedException();
		}

		public override MacdIndicatorResult Get(decimal?[] source, MacdOptions options)
		{
			throw new System.NotImplementedException();
		}
	}
}