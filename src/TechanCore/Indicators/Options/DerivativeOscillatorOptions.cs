using Contracts.Trading;

namespace TechanCore.Indicators.Options
{
	public class DerivativeOscillatorOptions : IOptionsSet
	{
		public int EmaFastPeriod { get; set; }
		public int EmaSlowPeriod { get; set; }
		
		public int SmaPeriod { get; set; }
		
		public int RsiPeriod { get; set; }
	}
}