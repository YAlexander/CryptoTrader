using core.Abstractions;

namespace Core.Trading.Indicators.Options
{
	public class MinusDiOptions : IIndicatorOptions
	{
		public int Period { get; }

		public MinusDiOptions (int period)
		{
			Period = period;
		}

		public dynamic Options => this;
	}
}
