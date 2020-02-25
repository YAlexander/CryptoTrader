using core.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace core.Extensions
{
	public static class CandleExtensions
	{
		public static List<decimal> High (this IEnumerable<ICandle> source) => source.Select(x => x.High).ToList();

		public static List<decimal> Low (this IEnumerable<ICandle> source) => source.Select(x => x.Low).ToList();

		public static List<decimal> Open (this IEnumerable<ICandle> source) => source.Select(x => x.Open).ToList();

		public static List<decimal> Close (this IEnumerable<ICandle> source) => source.Select(x => x.Close).ToList();

		public static List<decimal> Hl2 (this IEnumerable<ICandle> source) => source.Select(x => (x.High + x.Low) / 2).ToList();

		public static List<decimal> Hlc3 (this IEnumerable<ICandle> source) => source.Select(x => (x.High + x.Low + x.Close) / 3).ToList();
	}
}
