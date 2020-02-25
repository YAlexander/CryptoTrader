using Contracts.Trading;

namespace TechanCore.Indicators.Results
{
	public class StochasticOscillatorResult : IResultSet
	{
		public decimal?[] K { get; set; }
		
		public decimal?[] D { get; set; }
	}
}