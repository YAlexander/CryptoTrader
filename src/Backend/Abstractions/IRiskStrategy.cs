using Abstractions.Entities;

namespace Abstractions
{
	public interface IRiskStrategy
	{
		string Name { get; }
		
		ITradingContext Process (ITradingContext context, IStrategyInfo info);
	}
}