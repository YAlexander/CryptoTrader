using System.Collections.Generic;
using System.Linq;
using Contracts;

namespace TechanCore.Indicators.Helpers
{
	public static class DataHelper
	{
		public static decimal?[] High(this IEnumerable<ICandle> source) => source.Select(x => (decimal?)x.High).ToArray();

		public static decimal?[] Low(this IEnumerable<ICandle> source) => source.Select(x => (decimal?)x.Low).ToArray();

		public static decimal?[] Open(this IEnumerable<ICandle> source) => source.Select(x => (decimal?)x.Open).ToArray();

		public static decimal?[] Close(this IEnumerable<ICandle> source) => source.Select(x => (decimal?)x.Close).ToArray();

		// public static IEnumerable<bool> Crossunder (this decimal?[] source, decimal value)
		// {
		// 	List<bool> result = new List<bool>();
		//
		// 	for (int i = 0; i < source.Length; i++)
		// 	{
		// 		if (i == 0)
		// 		{
		// 			result.Add(false);
		// 		}
		// 		else
		// 		{
		// 			result.Add(source[i] < value && source[i - 1] >= value);
		// 		}
		// 	}
		//
		// 	return result;
		// }

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