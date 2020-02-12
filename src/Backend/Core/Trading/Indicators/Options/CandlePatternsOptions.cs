using Contracts.Trading;

namespace Core.Trading.Indicators.Options
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
