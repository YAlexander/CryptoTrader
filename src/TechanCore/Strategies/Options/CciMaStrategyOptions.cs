using TechanCore.Enums;

namespace TechanCore.Strategies.Options
{
	public class CciMaStrategyOptions : IStrategyOption
	{
		public MaTypes MaType { get; set; }
		public int CciPeriod { get; set; }
		public int MaFast { get; set; }
		public int MaSlow { get; set; }
	}
}