using System.Collections.Generic;

namespace core.Trading.Models
{
	public class BbandItem
	{
		public List<decimal?> UpperBand { get; set; }
		public List<decimal?> MiddleBand { get; set; }
		public List<decimal?> LowerBand { get; set; }
	}
}
