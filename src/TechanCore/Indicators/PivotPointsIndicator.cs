using System;
using System.Linq;
using System.Security.Cryptography;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
	public class PivotPointsIndicator : BaseIndicator<PivotPointsOptions, PivotPointsResults>
	{
		public override string Name { get; } = "Pivot Points (PP) Indicator";

		public override PivotPointsResults Get(ICandle[] source, PivotPointsOptions options)
		{
			PivotPointsResult[] results = new PivotPointsResult[source.Length];
			ICandle[] pivotCandles = source.GroupCandles(GetPivotTimeframe(source[0].TimeFrame));

			int daysPeriod = pivotCandles[0].TimeFrame == Timeframes.DAY ? 1 : 7;
			int position = 0;

			for (int i = 0; i < source.Length; i++)
			{
				var pivotCandle = pivotCandles[i];

				PivotPointsResult currentPivot = null;

				switch (options.Type)
				{
					case PivotPointsTypes.TRADITIONAL:
						{
							decimal p = (pivotCandle.High + pivotCandle.Low + pivotCandle.Close) / 3;

							decimal r1 = p * 2 - pivotCandle.Low;
							decimal s1 = p * 2 - pivotCandle.High;

							decimal r2 = p + r1 - s1;
							decimal s2 = p - r1 - s1;

							decimal r3 = pivotCandle.High + 2 * (p - pivotCandle.Low);
							decimal s3 = pivotCandle.Low - 2 * (pivotCandle.High - p);

							currentPivot = new PivotPointsResult
							{
								P = p,
								R1 = r1,
								R2 = r2,
								R3 = r3,
								S1 = s1,
								S2 = s2,
								S3 = s3
							};
						}
						break;

					case PivotPointsTypes.CAMARILLA:
						{
							decimal r1 = (pivotCandle.High - pivotCandle.Low) * 1.1m / 12 + pivotCandle.Close;
							decimal s1 = pivotCandle.Close - (pivotCandle.High - pivotCandle.Low) * 1.1m / 12;

							decimal r2 = (pivotCandle.High - pivotCandle.Low) * 1.1m / 6 + pivotCandle.Close;
							decimal s2 = pivotCandle.Close - (pivotCandle.High - pivotCandle.Low) * 1.1m / 6;

							decimal r3 = (pivotCandle.High - pivotCandle.Low) * 1.1m / 4 + pivotCandle.Close;
							decimal s3 = pivotCandle.Close - (pivotCandle.High - pivotCandle.Low) * 1.1m / 4;

							decimal r4 = (pivotCandle.High - pivotCandle.Low) * 1.1m / 2 + pivotCandle.Close;
							decimal s4 = pivotCandle.Close - (pivotCandle.High - pivotCandle.Low) * 1.1m / 2;

							decimal r5 = (pivotCandle.High / pivotCandle.Low) * pivotCandle.Close;
							decimal s5 = 2 * pivotCandle.Close - r5;

							currentPivot = new PivotPointsResult
							{
								R1 = r1,
								R2 = r2,
								R3 = r3,
								R4 = r4,
								R5 = r5,
								S1 = s1,
								S2 = s2,
								S3 = s3,
								S4 = s4,
								S5 = s5
							};
						}
						break;

					case PivotPointsTypes.CLASSIC:
						{
							decimal p = (pivotCandle.High + pivotCandle.Low + pivotCandle.Close) / 3;

							decimal r1 = 2 * p - pivotCandle.Low;
							decimal s1 = 2 * p - pivotCandle.High;

							decimal r2 = p + (pivotCandle.High - pivotCandle.Low);
							decimal s2 = p - (pivotCandle.High - pivotCandle.Low);

							decimal r3 = r1 + (pivotCandle.High - pivotCandle.Low);
							decimal s3 = s1 - (pivotCandle.High - pivotCandle.Low);

							currentPivot = new PivotPointsResult
							{
								P = p,
								R1 = r1,
								R2 = r2,
								R3 = r3,
								S1 = s1,
								S2 = s2,
								S3 = s3
							};
						}
						break;

					case PivotPointsTypes.DEMARK:
						{
							decimal p;

							if (pivotCandle.Close > pivotCandle.Open)
							{
								p = 2 * pivotCandle.High + pivotCandle.Low + pivotCandle.Close;
							}
							else if (pivotCandle.Close < pivotCandle.Open)
							{
								p = pivotCandle.High + 2 * pivotCandle.Low + pivotCandle.Close;
							}
							else
							{
								p = pivotCandle.High + pivotCandle.Low + 2 * pivotCandle.Close;
							}

							decimal s1 = p / 2 - pivotCandle.Low;
							decimal r1 = p / 2 + pivotCandle.High;

							currentPivot = new PivotPointsResult
							{
								P = p,
								R1 = r1,
								S1 = s1,
							};
						}
						break;

					case PivotPointsTypes.FIBONACCI:
						{
							decimal p = (pivotCandle.High + pivotCandle.Low + pivotCandle.Close) / 3;

							decimal r1 = p + 0.382m * (pivotCandle.High - pivotCandle.Low);
							decimal s1 = p - 0.382m * (pivotCandle.High - pivotCandle.Low);

							decimal r2 = p + 0.618m * (pivotCandle.High - pivotCandle.Low);
							decimal s2 = p - 0.618m * (pivotCandle.High - pivotCandle.Low);

							decimal r3 = p + (pivotCandle.High - pivotCandle.Low);
							decimal s3 = p - (pivotCandle.High - pivotCandle.Low);

							currentPivot = new PivotPointsResult
							{
								P = p,
								R1 = r1,
								R2 = r2,
								R3 = r3,
								S1 = s1,
								S2 = s2,
								S3 = s3
							};
						}
						break;

					case PivotPointsTypes.WOODIE:
						{
							decimal p = (pivotCandle.High + pivotCandle.Low + 2 * pivotCandle.Close) / 4;

							decimal r1 = p * 2 - pivotCandle.Low;
							decimal s1 = p * 2 - pivotCandle.High;

							decimal r2 = p + pivotCandle.High - pivotCandle.Low;
							decimal s2 = p - pivotCandle.High + pivotCandle.Low;

							currentPivot = new PivotPointsResult
							{
								P = p,
								R1 = r1,
								R2 = r2,
								S1 = s1,
								S2 = s2,
							};
						}
						break;

					default:
						throw new Exception($"Unsupported Pivot Type {options.Type}");
				}

				int numberOfCandles = source.Count(x => x.Time >= pivotCandle.Time && x.Time < pivotCandle.Time.AddDays(daysPeriod));
				for (int j = position; j < position + numberOfCandles; j++)
				{
					results[j] = currentPivot;
				}

				position += numberOfCandles;
			}

			return new PivotPointsResults { Pivots = results };
		}

		public override PivotPointsResults Get(decimal[] source, PivotPointsOptions options)
		{
			throw new System.NotImplementedException();
		}

		public override PivotPointsResults Get(decimal?[] source, PivotPointsOptions options)
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
