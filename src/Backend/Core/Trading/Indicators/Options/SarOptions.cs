using Contracts.Trading;

namespace Core.Trading.Indicators.Options
{
	public class SarOptions : IIndicatorOptions
	{
		public double AccelerationFactor { get; }
		public double MaximumAccelerationFactor { get; }

		public dynamic Options => this;

		public SarOptions (double accelerationFactor, double maximumAccelerationFactor)
		{
			AccelerationFactor = accelerationFactor;
			MaximumAccelerationFactor = maximumAccelerationFactor;
		}
	}
}
