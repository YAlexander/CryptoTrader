using Abstractions.Entities;

namespace Abstractions
{
	public interface IRiskManager
	{
		string Name { get; set; }
		ITradingContext Process (ITradingContext context, IStrategyInfo info);
	}
}