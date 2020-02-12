using Contracts.Trading;

namespace Core.Trading.Indicators.Options
{
	public class MacdOptions : IIndicatorOptions
	{
		public int SlowPeriod { get; }
		public int FastPeriod { get; }
		public int SignalPeriod { get; }

		public MacdOptions (int slowPeriod, int fastPeriod, int signalPeriod)
		{
			SlowPeriod = slowPeriod;
			FastPeriod = fastPeriod;
			SignalPeriod = signalPeriod;
		}

		public dynamic Options => this;
	}
}
