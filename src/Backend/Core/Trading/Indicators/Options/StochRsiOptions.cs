using Contracts.Trading;
using Contracts.TypeCodes;

namespace Core.Trading.Indicators.Options
{
	public class StochRsiOptions : IIndicatorOptions
	{
		public int OptInTimePeriod { get; }
		public ICandleVariableCode Type { get; }
		public int FastKPeriod { get; }
		public int FastDPeriod { get; }
		public TicTacTec.TA.Library.Core.MAType FastDmaType { get; }

		public dynamic Options => this;

		public StochRsiOptions (int optInTimePeriod, ICandleVariableCode type, int fastKPeriod, int fastDPeriod, TicTacTec.TA.Library.Core.MAType fastDmaType)
		{
			OptInTimePeriod = optInTimePeriod;
			Type = type;
			FastDmaType = fastDmaType;
			FastKPeriod = fastKPeriod;
			FastDPeriod = fastDPeriod;
		}
	}
}