namespace core.Infrastructure.Database.Entities
{
	public class Balance : BaseEntity
	{
		public int ExchangeCode { get; set; }
		public string Asset { get; set; }
		public decimal Total { get; set; }
		public decimal Available { get; set; }
	}
}
