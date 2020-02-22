using System.Threading.Tasks;

namespace Contracts.Trading
{
	public interface IRisk
	{
		Task Get(ICandle[] candles, IStrategyInfo info, decimal balance, ref IRiskResult result);
	}
}