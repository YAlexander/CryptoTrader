using Contracts.Trading;
using Contracts.TypeCodes;

namespace Core.Trading.Indicators.Options
{
	public class TemaOptions : IIndicatorOptions
	{
		public int Period { get; }
		public ICandleVariableCode Type { get; }

		public TemaOptions (int period, ICandleVariableCode type)
		{
			Period = period;
			Type = type;
		}

		public dynamic Options => this;
	}
}
