using System;
using System.Collections.Generic;
using System.Text;

namespace core.Infrastructure.Database.Entities
{
	public class Book : BaseEntity
	{
		public int ExchangeCode { get; set; }
		public string Symbol { get; set; }
		public decimal BestAskPrice { get; set; }
		public decimal BestAskQuantity { get; set; }
		public decimal BestBidPrice { get; set; }
		public decimal BestBidQuantity { get; set; }
	}
}
