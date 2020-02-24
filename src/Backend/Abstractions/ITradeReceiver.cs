using System.Threading.Tasks;
using Contracts.Trading;
using Orleans;

namespace Abstractions
{
	public interface ITradeReceiver : IGrainWithIntegerKey
	{
		Task<bool> Receive(ITradeInfo trade);
	}
}