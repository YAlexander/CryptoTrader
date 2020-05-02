using System.Collections.Generic;
using TechanCore.Helpers;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
	public class HeikenAshiIndicator : BaseIndicator<HeikenAshiOptions, HeikenAshiResult>
	{
		public override string Name { get; }
		public override HeikenAshiResult Get(ICandle[] source, HeikenAshiOptions options)
		{
			IEnumerable<ICandle> candles = options.IsSmoothed
				? source.HeikenAshiSmoothed(options.MaType, options.MaPeriod)
				: source.HeikenAshi();

			return new HeikenAshiResult() { Candles = candles };
		}

		public override HeikenAshiResult Get(decimal[] source, HeikenAshiOptions options)
		{
			throw new System.NotImplementedException();
		}

		public override HeikenAshiResult Get(decimal?[] source, HeikenAshiOptions options)
		{
			throw new System.NotImplementedException();
		}
	}
}