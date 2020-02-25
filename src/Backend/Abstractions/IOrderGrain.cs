using System.Threading.Tasks;
using Contracts.Trading;
using Orleans;

namespace Abstractions
{
	public interface IOrderGrain : IGrainWithIntegerCompoundKey
	{
		Task<bool> Receive(IOrder order);
	}
}