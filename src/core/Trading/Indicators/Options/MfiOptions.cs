using core.Abstractions;

namespace Core.Trading.Indicators.Options
{
	public class MfiOptions : IIndicatorOptions
	{
		public int Period { get; }

		public MfiOptions (int period)
		{
			Period = period;
		}

		public dynamic Options => this;
	}
}
