using Contracts.Trading;

namespace Core.Trading.Indicators.Options
{
	public class PlusDiOptions : IIndicatorOptions
	{
		public int Period { get; }

		public PlusDiOptions (int period)
		{
			Period = period;
		}

		public dynamic Options => this;
	}
}
