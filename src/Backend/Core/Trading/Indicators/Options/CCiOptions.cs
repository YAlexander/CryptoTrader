using Contracts.Trading;

namespace Core.Trading.Indicators.Options
{
	public class CciOptions : IIndicatorOptions
	{
		public int Period { get; }

		public CciOptions (int period)
		{
			Period = period;
		}

		public dynamic Options => this;
	}
}
