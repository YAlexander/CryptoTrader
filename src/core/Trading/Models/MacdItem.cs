using System.Collections.Generic;

namespace core.Trading.Models
{
	public class MacdItem
	{
		public List<decimal?> Macd { get; set; }
		public List<decimal?> Signal { get; set; }
		public List<decimal?> Hist { get; set; }
	}
}
