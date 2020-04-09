using Abstractions.Entities;

namespace Abstractions
{
	public interface IRiskManager
	{
		ITradingContext Process (ITradingContext context, IStrategyInfo info);
	}
}