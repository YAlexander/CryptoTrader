using System.Threading.Tasks;

namespace Contracts.Trading
{
	public interface IRisk
	{
		(int, bool) Get(ICandle[] candles);
	}
}