using System.Collections.Generic;
using System.Linq;

namespace TechanCore.Indicators.Helpers
{
	public static class DataHelper
	{
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