using System.Linq;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
	public class PivotPointsIndicator: BaseIndicator<EmptyOption, PivotPointsResults>
	{
		public override string Name { get; } = "Pivot Points (PP) Indicator";
		
		public override PivotPointsResults Get(ICandle[] source, EmptyOption options)
		{
			PivotPointsResult[] results = new PivotPointsResult[source.Length];
			ICandle[] pivotCandles = source.GroupCandles(GetPivotTimeframe(source[0].TimeFrame));

			int daysPeriod = pivotCandles[0].TimeFrame == Timeframes.DAY ? 1 : 7;
			int position = 0;

			for (int i = 0; i < source.Length; i++)
			{
				var pivotCandle = pivotCandles[i];
				
				decimal p = (pivotCandle.High + pivotCandle.Low + pivotCandle.Close) / 3;
				decimal r1 = p * 2 - pivotCandle.Low;
				decimal s1 = p * 2 - pivotCandle.High;
				decimal r2 = p + pivotCandle.High - pivotCandle.Low;
				decimal s2 = p - pivotCandle.Low + pivotCandle.High;

				PivotPointsResult currentPivot = new PivotPointsResult
				{
					P = p,
					R1 = r1,
					R2 = r2,
					S1 = s1,
					S2 = s2
				};
				
				int numberOfCandles = source.Count(x => x.Time >= pivotCandle.Time && x.Time < pivotCandle.Time.AddDays(daysPeriod));
				for (int j = position; j < position + numberOfCandles; j++)
				{
					results[j] = currentPivot;
				}

				position += numberOfCandles;
			}
			
			return new PivotPointsResults { Pivots = results };
		}

		public override PivotPointsResults Get(decimal[] source, EmptyOption options)
		{
			throw new System.NotImplementedException();
		}

		public override PivotPointsResults Get(decimal?[] source, EmptyOption options)
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
