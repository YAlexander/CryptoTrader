using core.Abstractions;

namespace Core.Trading.Indicators.Options
{
	public class AtrOptions : IIndicatorOptions
	{
		public int Period { get; }

		public AtrOptions (int period)
		{
			Period = period;
		}

		public dynamic Options => this;
	}
}
