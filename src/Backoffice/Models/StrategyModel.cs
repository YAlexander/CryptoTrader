using core.Abstractions.TypeCodes;
using core.TypeCodes;
using System.ComponentModel.DataAnnotations;

namespace Backoffice.Models
{
	public class StrategyModel
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public int OptimalPeriodCode { get; set; } = PeriodCode.UNKNOWN.Code;

		[Display(Name = "Timeframe")]
		public string OptimalPeriodName { get; set; } = PeriodCode.UNKNOWN.Description;

		[Display(Name = "Candles")]
		public int NumberOfCandles { get; set; }

		[Display(Name = "Is Enabled")]
		public bool IsEnabled { get; set; }
	}
}
