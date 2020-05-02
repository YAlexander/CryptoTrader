using Abstractions.Enums;
using System;
using System.Collections.Generic;

namespace Site.Core.Reports.ReportsData
{
	public class TradingJournalData : IReport
	{
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }

		public Exchanges Exchange { get; set; }

		public double ProfitFactor { get; set; }

		public List<TradingJournalLine> Lines { get; } = new List<TradingJournalLine>();
	}
}
