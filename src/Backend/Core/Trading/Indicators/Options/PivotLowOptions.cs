using Contracts.Trading;

namespace Core.Trading.Indicators.Options
{
	public class PivotLowOptions : IIndicatorOptions
	{
		public int BarsLeft { get; }
		public int BarsRight { get; }
		public bool FillNullValues { get; }

		public dynamic Options => this;

		public PivotLowOptions (int barsLeft, int barsRight, bool fillNullValues)
		{
			BarsLeft = barsLeft;
			BarsRight = barsRight;
			FillNullValues = fillNullValues;
		}
	}
}
