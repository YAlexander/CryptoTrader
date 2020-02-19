using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class BollingerAwesomeMacdOptions : IStrategyOption
	{
		public int FastPeriod { get; set; }
		public int SlowPeriod { get; set; }
		public int SignalPeriod { get; set; }
		
		public int BollingerPeriod { get; set; }
		public int BollingerDeviationDown { get; set; }
		public int BollingerDeviationUp { get; set; }
		
		public int AwesomeFastPeriod { get; set; }
		public int AwesomeSlowPeriod { get; set; }
		
		public int EmaPeriod { get; set; }
		
		public int SmaFastPeriod { get; set; }
		public int SmaSlowPeriod { get; set; }
	}
}