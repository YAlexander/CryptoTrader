using core.Abstractions;
using core.Abstractions.TypeCodes;

namespace core.Trading.Indicators.Options
{
	public class SmaOptions : IIndicatorOptions
	{
		public int Period { get; }
		public ICandleVariableCode Type { get; }

		public SmaOptions (int period, ICandleVariableCode type)
		{
			Period = period;
			Type = type;
		}

		public dynamic Options => this;
	}
}
