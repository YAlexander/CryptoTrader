using System;
using System.Collections.Generic;
using Abstractions.Entities;
using Abstractions.Enums;

namespace Persistence.Entities
{
	public class Deal : BaseEntity<Guid>, IDeal
	{
		public Exchanges Exchange { get; set; }
		public Assets Asset1 { get; set; }
		public Assets Asset2 { get; set; }

		public DealPositions Position { get; set; }
		public DealStatus Status { get; set; }
		public string StatusDescription { get; set; }
		
		private List<Order> Orders { get; set; } = new List<Order>();
		
		public decimal? AvgPrice { get; set; }
		public decimal? TotalAmount { get; set; }
		
		public decimal? TakeProfit { get; set; }
		public decimal? StopLoss { get; set; }
		
		public decimal? OrderLimit { get; set; }
		public decimal? PositionLimit { get; set; }
	}
}