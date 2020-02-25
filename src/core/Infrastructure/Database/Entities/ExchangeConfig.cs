using System.Collections.Generic;

namespace core.Infrastructure.Database.Entities
{
	public class ExchangeConfig : BaseEntity
	{
		public int ExchangeCode { get; set; }
		public bool IsEnabled { get; set; }

		public string ApiKey { get; set; }
		public string ApiSecret { get; set; }

		public List<PairConfig> Pairs { get; set; } = new List<PairConfig>();
	}
}
