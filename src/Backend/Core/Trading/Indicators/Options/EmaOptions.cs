using Contracts.Trading;
using Contracts.TypeCodes;

namespace Core.Trading.Indicators.Options
{
	public class EmaOptions : IIndicatorOptions
	{
		public int Period { get; }
		public ICandleVariableCode Type { get; }

		public EmaOptions (int period, ICandleVariableCode type)
		{
			Period = period;
			Type = type;
		}

		public dynamic Options => this;
	}
}
