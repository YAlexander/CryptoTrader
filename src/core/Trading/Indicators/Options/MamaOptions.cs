using core.Abstractions;

namespace Core.Trading.Indicators.Options
{
	public class MamaOptions : IIndicatorOptions
	{
		public double FastLimit { get; }
		public double SlowLimit { get; }

		public MamaOptions (double fastLimit, double slowLimit)
		{
			FastLimit = fastLimit;
			SlowLimit = slowLimit; 
		}

		public dynamic Options => this;
	}
}
