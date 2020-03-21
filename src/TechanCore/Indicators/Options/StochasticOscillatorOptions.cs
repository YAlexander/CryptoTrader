namespace TechanCore.Indicators.Options
{
	public class StochasticOscillatorOptions : IOptionsSet 
	{
		public int Period { get; set; }
		
		public int EmaPeriod { get; set; }
	}
}