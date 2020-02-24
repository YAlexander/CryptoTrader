using System.Threading.Tasks;
using Contracts;
using Orleans;

namespace Abstractions
{
	public interface ICandleReceiver : IGrainWithIntegerKey
	{
		Task<bool> Receive(ICandle candle);
	}
}