using System;
using Abstractions.Enums;

namespace Site.Core.Reports.ReportsData
{
	public class TradingJournalLine
	{
		public string DealId { get; set; }
		public Assets Asset1 { get; set; }
		public Assets Asset2 { get; set; }
		
		public decimal Volume { get; set; }

		public DealPositions Position { get; set; }
		public DateTime OpenTime { get; set; }
		public DateTime CloseTime { get; set; }

		public decimal AvgEnterPrice { get; set; }
		public decimal AvgExitPrice { get; set; }

		public string ExitReason { get; set; }

		public decimal? StopLossLevel { get; set; }
		public decimal? TakeProfitLevel { get; set; }
		public bool TrailingStopUsed { get; set; }
		public decimal AtrLevel { get; set; }

		public decimal AvgComission { get; set; }

		public bool ManualTrading { get; set; }
		public string StrategyName { get; set; }
	}
}
