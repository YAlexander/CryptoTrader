using Contracts.Enums;
using Contracts.Trading;

namespace core.Trading.TAIndicators.Options
{
	public class SmaOptions : IOptionsSet
	{
		public int Period { get; set; }

		public CandleVariables? CandleVariable { get; set; }
	}
}