using System.Collections.Generic;
using System.Linq;
using Contracts;

namespace TechanCore.Helpers
{
	public static class DataHelper
	{
		public static decimal[] High(this IEnumerable<ICandle> source) => source.Select(x => x.High).ToArray();
		
		public static decimal[] Low(this IEnumerable<ICandle> source) => source.Select(x => x.Low).ToArray();

		public static decimal[] Open(this IEnumerable<ICandle> source) => source.Select(x => x.Open).ToArray();

		public static decimal[] Close(this IEnumerable<ICandle> source) => source.Select(x => x.Close).ToArray();
		
		public static decimal[] Volume(this IEnumerable<ICandle> source) => source.Select(x => x.Volume).ToArray();
		
		public static decimal[] AverageExtremum(this IEnumerable<ICandle> source) => source.Select(x => (x.High + x.Low) / 2).ToArray();
		
		public static decimal[] Hlc3(this IEnumerable<ICandle> source) =>  source.Select(x => (x.High + x.Low + x.Close) / 3).ToArray();

		public static decimal?[] ToNullable(this IEnumerable<decimal> source) => source.Select(x => (decimal?) x).ToArray();


		public static IEnumerable<bool> Crossunder (this decimal?[] source, decimal?[] value)
		{
			List<bool> result = new List<bool>();

			for (int i = 0; i < source.Length; i++)
			{
				if (i == 0)
				{
					result.Add(false);
				}
				else
				{
					result.Add(source[i] < value[i] && source[i - 1] >= value[i - 1]);
				}
			}

			return result;
		}
		
		public static IEnumerable<bool> Crossunder (this decimal?[] source, decimal[] value)
		{
			List<bool> result = new List<bool>();

			for (int i = 0; i < source.Length; i++)
			{
				if (i == 0)
				{
					result.Add(false);
				}
				else
				{
					result.Add(source[i] < value[i] && source[i - 1] >= value[i - 1]);
				}
			}

			return result;
		}
		
		public static IEnumerable<bool> Crossunder (this decimal[] source, decimal?[] value)
		{
			List<bool> result = new List<bool>();

			for (int i = 0; i < source.Length; i++)
			{
				if (i == 0)
				{
					result.Add(false);
				}
				else
				{
					result.Add(source[i] < value[i] && source[i - 1] >= value[i - 1]);
				}
			}

			return result;
		}
		
		public static IEnumerable<bool> Crossover (this decimal?[] source, decimal?[] value)
		{
			List<bool> result = new List<bool>();

			for (int i = 0; i < source.Length; i++)
			{
				if (i == 0)
				{
					result.Add(false);
				}
				else
				{
					result.Add(source[i] > value[i] && source[i - 1] <= value[i - 1]);
				}
			}

			return result;
		}
	}
}