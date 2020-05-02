using TechanCore.Enums;

namespace TechanCore.Strategies.Options
{
	public class HeikenAshiStrategyOptions : IStrategyOption
	{
		public MaTypes MaType { get; set; }
		public int MaPeriod { get; set; }
	}
}
