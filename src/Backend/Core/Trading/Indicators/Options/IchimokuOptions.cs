using Contracts.Trading;

namespace Core.Trading.Indicators.Options
{
	public class IchimokuOptions : IIndicatorOptions
	{
		public int ConversionLinePeriod { get; }
		public int BaseLinePeriod { get; }
		public int LaggingSpanPeriods { get; }
		public int Displacement { get; }

		public dynamic Options => this;

		public IchimokuOptions (int conversionLinePeriod, int baseLinePeriod, int laggingSpanPeriods, int displacement)
		{
			ConversionLinePeriod = conversionLinePeriod;
			BaseLinePeriod = baseLinePeriod;
			LaggingSpanPeriods = laggingSpanPeriods;
			Displacement = displacement;
		}
	}
}
