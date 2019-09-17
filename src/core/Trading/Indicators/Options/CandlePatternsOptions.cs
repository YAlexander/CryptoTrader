using core.Abstractions;

namespace core.Trading.Indicators.Options
{
	public class CandlePatternsOptions : IIndicatorOptions
	{
		public decimal DojiSize { get; }

		public CandlePatternsOptions (decimal dojiSize)
		{
			DojiSize = dojiSize;
		}

		public dynamic Options => this;
	}
}
