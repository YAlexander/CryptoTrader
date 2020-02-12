using Contracts.Trading;

namespace Core.Trading.Indicators.Options
{
	public class StochOptions : IIndicatorOptions
	{
		public int FastKPeriod { get; }
		public int SlowKPeriod { get; }
		public TicTacTec.TA.Library.Core.MAType SlowKmaType { get; }
		public int SlowDPeriod { get; }
		public TicTacTec.TA.Library.Core.MAType SlowDmaType { get; }

		public dynamic Options => this;

		public StochOptions (int fastKPeriod, int slowKPeriod, TicTacTec.TA.Library.Core.MAType slowKmaType, int slowDPeriod, TicTacTec.TA.Library.Core.MAType slowDmaType)
		{
			FastKPeriod = fastKPeriod;
			SlowKPeriod = slowKPeriod;
			SlowKmaType = slowKmaType;
			SlowDPeriod = slowDPeriod;
			SlowDmaType = slowDmaType;
		}
	}
}