using System.Threading.Tasks;
using Contracts;
using Contracts.Enums;
using Orleans;

namespace Abstractions
{
	public interface ICandleReceiver : IGrainWithIntegerKey
	{
		Task Receive(Exchanges exchange, Assets asset1, Assets asset2, ICandle candle);
	}
}