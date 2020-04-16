using System.Linq;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
	public class PivotPointsIndicator: BaseIndicator<EmptyOption, PivotPointsResult>
	{
		public override string Name { get; } = "Pivot Points (PP) Indicator";
		
		public override PivotPointsResult Get(ICandle[] source, EmptyOption options)
		{
			// TODO: Calculate PP for all candles, not only for last period
			Timeframes pivotTimeFrame = GetPivotTimeframe(source[0].TimeFrame);
			ICandle pivotCandle = source.GroupCandles(pivotTimeFrame).Last();

			decimal p = (pivotCandle.High + pivotCandle.Low + pivotCandle.Close) / 3;
			decimal r1 = p * 2 - pivotCandle.Low;
			decimal s1 = p * 2 - pivotCandle.High;
			decimal r2 = p + pivotCandle.High - pivotCandle.Low;
			decimal s2 = p - pivotCandle.Low + pivotCandle.High;
			
			return new PivotPointsResult
			{
				P = p,
				R1 = r1,
				R2 = r2,
				S1 = s1,
				S2 = s2
			};
		}

		public override PivotPointsResult Get(decimal[] source, EmptyOption options)
		{
			throw new System.NotImplementedException();
		}

		public override PivotPointsResult Get(decimal?[] source, EmptyOption options)
		{
			throw new System.NotImplementedException();
		}

		private Timeframes GetPivotTimeframe(Timeframes candleTimeframe)
		{
			return candleTimeframe switch
			{
				Timeframes.DAY => Timeframes.WEEK,
				_ => Timeframes.DAY
			};
		}
	}
}
