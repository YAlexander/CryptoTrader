using core.Abstractions;

namespace Core.Trading.Indicators.Options
{
	public class StochFastOptions : IIndicatorOptions
	{
		public int FastKPeriod { get; }
		public int FastDPeriod { get; }
		public TicTacTec.TA.Library.Core.MAType FastDmaType { get; }

		public dynamic Options => this;

		public StochFastOptions (int fastKPeriod, int fastDPeriod, TicTacTec.TA.Library.Core.MAType fastDmaType)
		{
			FastDmaType = fastDmaType;
			FastKPeriod = fastKPeriod;
			FastDPeriod = fastDPeriod;
		}
	}
}