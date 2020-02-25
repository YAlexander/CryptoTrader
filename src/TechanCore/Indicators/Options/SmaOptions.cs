using Contracts.Enums;
using Contracts.Trading;
using TechanCore.Enums;

namespace TechanCore.Indicators.Options
{
	public class SmaOptions : IOptionsSet
	{
		public int Period { get; set; }

		public CandleVariables? CandleVariable { get; set; }
	}
}