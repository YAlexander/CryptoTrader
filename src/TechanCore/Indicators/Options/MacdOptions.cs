namespace TechanCore.Indicators.Options
{
	public class MacdOptions : IOptionsSet
	{
		public int FastPeriod { get; set; }
		public int SlowPeriod { get; set; }
		public int SignalPeriod { get; set; }
	}
}