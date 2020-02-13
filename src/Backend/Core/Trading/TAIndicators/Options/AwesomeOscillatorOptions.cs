using Contracts.Trading;

namespace core.Trading.TAIndicators.Options
{
	public class AwesomeOscillatorOptions : IOptionsSet
	{
		public int SlowSmaPeriod { get; set; }
		
		public int FastSmaPeriod { get; set; }
	}
}