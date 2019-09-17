using System.Collections.Generic;

namespace core.Trading.Models
{
	public class IchimokuItem
	{
		public List<decimal?> TenkanSen { get; set; }
		public List<decimal?> KijunSen { get; set; }
		public List<decimal?> SenkouSpanA { get; set; }
		public List<decimal?> SenkouSpanB { get; set; }
		public List<decimal?> ChikouSpan { get; set; }
	}
}
