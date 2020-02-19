using Contracts.Trading;

namespace TechanCore.Indicators.Results
{
	public class SeriesIndicatorResult : IResultSet
	{
		public decimal?[] Result { get; set; }
	}
}