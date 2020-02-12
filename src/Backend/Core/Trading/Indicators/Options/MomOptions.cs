using Contracts.Trading;

namespace Core.Trading.Indicators.Options
{
	public class MomOptions : IIndicatorOptions
	{
		public int Period { get; }

		public MomOptions (int period)
		{
			Period = period;
		}

		public dynamic Options => this;
	}
}
