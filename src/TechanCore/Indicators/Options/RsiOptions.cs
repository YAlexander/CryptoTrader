using Contracts.Trading;

namespace TechanCore.Indicators.Options
{
	public class RsiOptions : IOptionsSet
	{
		public int Period { get; set; }
	}
}