using core.Abstractions;

namespace Core.Trading.Indicators.Options
{
	public class CmoOptions : IIndicatorOptions
	{
		public int Period { get; }

		public CmoOptions (int period)
		{
			Period = period;
		}

		public dynamic Options => this;
	}
}
