using System.Collections.Generic;
using Contracts.Trading;

namespace core.Trading.TAIndicators.Results
{
	public class DefaultIndicatorResult : IResultSet
	{
		public decimal?[] Result { get; set; }
	}
}