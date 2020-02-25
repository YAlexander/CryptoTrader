using System.Threading.Tasks;

namespace Contracts.Trading
{
	public interface ITradingConstraint<T>
	{
		Task<T> Get (ICandle[] candles, IStrategyInfo info, decimal[] balances);
	}
}