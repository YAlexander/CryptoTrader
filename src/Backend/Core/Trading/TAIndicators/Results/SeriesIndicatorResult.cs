using System.Collections.Generic;
using Contracts.Trading;

namespace core.Trading.TAIndicators.Results
{
	public class SeriesIndicatorResult : IResultSet
	{
		public decimal?[] Result { get; set; }
	}
}