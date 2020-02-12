using Contracts.Trading;

namespace Core.Trading.Indicators.Options
{
	public class AdxOptions
	{
		public int Period { get; }

		public AdxOptions (int period)
		{
			Period = period;
		}

		public dynamic Options => this;
	}
}
