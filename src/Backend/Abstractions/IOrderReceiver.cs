using System.Threading.Tasks;
using Contracts.Trading;
using Orleans;

namespace Abstractions
{
	public interface IOrderReceiver : IGrainWithIntegerKey
	{
		Task<bool> Receive(IOrder order);
	}
}