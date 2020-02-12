using System.Collections.Generic;

namespace Core.Trading.Models
{
	public class BbandItem
	{
		public List<decimal?> UpperBand { get; set; }
		public List<decimal?> MiddleBand { get; set; }
		public List<decimal?> LowerBand { get; set; }
	}
}
