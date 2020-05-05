using TechanCore.Enums;

namespace TechanCore.Strategies.Options
{
	public class AwesomeMaStrategyOptions : IStrategyOption
	{
		public int AwesomeFastPeriod { get; set; }
		public int AwesomeSlowPeriod { get; set; }

		public MaTypes MaType { get; set; }
		public int MaFastPeriod { get; set; }
		public int MaSlowPeriod { get; set; }
	}
}