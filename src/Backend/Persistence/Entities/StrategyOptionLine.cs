using Abstractions.Enums;

namespace Persistence.Entities
{
	public class StrategyOptionLine
	{
		public Exchanges Exchange { get; set; }
		public Assets Asset1 { get; set; }
		public Assets Asset2 { get; set; }
		public long StrategyId { get; set; }
		public string Options { get; set; }
	}
}