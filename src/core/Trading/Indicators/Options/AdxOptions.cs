using core.Abstractions;

namespace Core.Trading.Indicators.Options
{
	public class AdxOptions : IIndicatorOptions
	{
		public int Period { get; }

		public AdxOptions (int period)
		{
			Period = period;
		}

		public dynamic Options => this;
	}
}
