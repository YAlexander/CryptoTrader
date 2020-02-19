using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class BollingerBandsRsiOptions : IStrategyOption
	{
		public int BollingerPeriod { get; set; }
		public int BollingerDeviationDown { get; set; }
		public int BollingerDeviationUp { get; set; }
		
		public int RsiPeriod { get; set; }
	}
}