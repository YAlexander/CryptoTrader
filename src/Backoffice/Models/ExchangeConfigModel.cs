using System.Collections.Generic;
using core.Infrastructure.Database.Entities;
using core.TypeCodes;


namespace Backoffice.Models
{
	public class ExchangeConfigModel
	{
		public long? Id { get; set; }
		public int ExchangeCodeId { get; set; } = ExchangeCode.UNKNOWN.Code;
		public string ExchangeName { get; set; } = ExchangeCode.UNKNOWN.Description;

		public bool IsEnabled { get; set; }

		public string ApiKey { get; set; }
		public string ApiSecret { get; set; }

		public IEnumerable<PairConfig> Pairs { get; set; } = new List<PairConfig>();
	}
}
