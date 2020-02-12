using Contracts.Trading;

namespace Core.Trading.Indicators.Options
{
	public class RsiOptions : IIndicatorOptions
	{
		public int Period { get; }

		public RsiOptions (int period)
		{
			Period = period;
		}

		public dynamic Options => this;
	}
}
