using System.Collections.Generic;

namespace core.Abstractions
{
	public interface IIndicator
	{
		string Name { get; }

		dynamic Get (IEnumerable<ICandle> candles, IIndicatorOptions options = null);

		dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null);
	}
}
