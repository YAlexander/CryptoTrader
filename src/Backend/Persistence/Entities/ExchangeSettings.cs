using Abstractions.Enums;
using TechanCore.Enums;

namespace Persistence.Entities
{
	public interface IExchangeSettings
	{
		public Exchanges Exchange { get; set; }
		
		public Assets Asset1 { get; set; }
		public Assets Asset2 { get; set; }
		
		public Timeframes Timeframe { get; set; }

		public bool IsEnabled { get; set; }
		public bool IsTradingSimulation { get; set; }
		
		public int UpdatingInterval { get; set; }
	}
}