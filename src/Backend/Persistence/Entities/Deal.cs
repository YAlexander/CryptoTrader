using System.Collections.Generic;
using Abstractions.Enums;

namespace Persistence.Entities
{
	public class Deal : BaseEntity
	{
		public Exchanges Exchange { get; set; }
		public Assets Asset1 { get; set; }
		public Assets Asset2 { get; set; }

		public DealPositions Position { get; set; }
		public DealStatus Status { get; set; }
		public string StatusDescription { get; set; }
		
		private List<Order> Orders { get; set; } = new List<Order>();
	}
}