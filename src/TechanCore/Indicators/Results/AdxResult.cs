using Contracts.Trading;

namespace TechanCore.Indicators.Results
{
	public class AdxResult : IResultSet
	{
		public decimal?[] PlusDi { get; set; }
		public decimal?[] MinusDi { get; set; }
		public decimal?[] Adx { get; set; }
	}
}