using Abstractions.Entities;
using Abstractions.Enums;

namespace Persistence.Entities
{
	public class Balance : IBalance
	{
		public Exchanges Exchange { get; set; }
		public Assets Asset { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal LockedAmount { get; set; }
		public string WalletId { get; set; }
	}
}