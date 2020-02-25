using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class BullishEngulfingOptions : IStrategyOption
	{
		public int RsiOptions { get; set; }
	}
}