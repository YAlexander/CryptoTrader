using System;
using System.Collections.Generic;
using Abstractions.Entities;
using Abstractions.Enums;

namespace Persistence.Entities
{
	public class DealEntity : BaseEntity<Guid>, IDeal
	{
		public Exchanges Exchange { get; set; }
		public Assets Asset1 { get; set; }
		public Assets Asset2 { get; set; }

		public DealPositions? Position { get; set; }
		public DealStatus Status { get; set; }
		public string StatusDescription { get; set; }
		
		public decimal? AvgPrice { get; set; }
		public decimal? TotalAmount { get; set; }
		
		public List<IOrder> Orders { get; set; } = new List<IOrder>();
	}
}