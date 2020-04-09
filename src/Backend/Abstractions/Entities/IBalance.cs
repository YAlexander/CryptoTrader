using Abstractions.Enums;

namespace Abstractions.Entities
{
	public interface IBalance
	{
		Exchanges Exchange { get; set; }
		Assets Asset { get; set; }
		
		public decimal TotalAmount { get; set; }
		public decimal LockedAmount { get; set; }
		
		public string WalletId { get; set; }
	}
}