using System;
using Abstractions.Enums;

namespace Abstractions
{
	public interface ITrade
	{
		public Exchanges Exchange { get; set; }
		public Assets Asset1 { get; set; }
		public Assets Asset2 { get; set; }
		
		public DateTime Time { get; set; }
		
		public decimal Price { get; set; }
		public decimal Quantity { get; set; }
	}
}