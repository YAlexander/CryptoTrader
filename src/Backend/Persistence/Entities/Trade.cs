using System;
using Contracts.Enums;
using Contracts.Trading;

namespace Persistence.Entities
{
	public class Trade : BaseEntity, ITrade
	{
		public Exchanges Exchange { get; set; }
		public Assets Asset1 { get; set; }
		public Assets Asset2 { get; set; }
		
		public DateTime Time { get; set; }
		
		public decimal Price { get; set; }
		public decimal Quantity { get; set; }
	}
}
