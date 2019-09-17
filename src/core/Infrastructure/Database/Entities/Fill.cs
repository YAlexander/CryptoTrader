using System;
using System.Collections.Generic;
using System.Text;

namespace core.Infrastructure.Database.Entities
{
	public class Fill : BaseEntity
	{
		public string Asset { get; set; }
		public decimal Price { get; set; }
		public decimal Amount { get; set; }
		public decimal EstimatedFee { get; set; }
	}
}
