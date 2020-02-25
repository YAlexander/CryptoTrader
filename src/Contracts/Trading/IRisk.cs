using System.Threading.Tasks;

namespace Contracts.Trading
{
	public interface IRisk<T>
	{
		Task<T> Get (ICandle[] candles, IStrategyInfo info, decimal[] balances);
	}
}