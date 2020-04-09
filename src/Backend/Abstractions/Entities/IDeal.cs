using System;
using System.Collections.Generic;
using Abstractions.Enums;

namespace Abstractions.Entities
{
	public interface IDeal
	{
		Guid Id { get; set; }
		Exchanges Exchange { get; set; }
		Assets Asset1 { get; set; }
		Assets Asset2 { get; set; }

		DealPositions? Position { get; set; }
		DealStatus Status { get; set; }
		public string StatusDescription { get; set; }
		
		decimal? AvgPrice { get; set; }
		decimal? TotalAmount { get; set; }
		
		List<IOrder> Orders { get; set; }
	}
}