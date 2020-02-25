namespace core.Trading.Strategies.Presets
{
	public class AdxMomentumPreset
	{
		public int Adx { get; set; }
		public int PlusDi { get; set; }
		public int MinusDi { get; set; }
		public double AccelerationFactor;
		public double MaximumAccelerationFactor { get; set; }
		public int Mom { get; set; }
	}
}
