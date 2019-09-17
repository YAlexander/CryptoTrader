using core.Abstractions;

namespace Core.Trading.Indicators.Options
{
	public class FisherOptions : IIndicatorOptions
	{
		public int Period { get; }
		public bool RawValues { get; }

		public FisherOptions (int period, bool rawValues)
		{
			Period = period;
			RawValues = rawValues; 
		}

		public dynamic Options => this;
	}
}
