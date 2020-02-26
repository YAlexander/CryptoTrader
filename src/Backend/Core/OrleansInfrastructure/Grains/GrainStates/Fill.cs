using Contracts.Trading;
using Persistence.Entities;

namespace Core.OrleansInfrastructure.Grains.GrainStates
{
	public class Fill : BaseEntity, IFill
	{
		public decimal Price { get; set; }
		public decimal Quantity { get; set; }
		public decimal Fee { get; set; }
		public string FeeAsset { get; set; }
		public long TradeId { get; set; }
	}
}
