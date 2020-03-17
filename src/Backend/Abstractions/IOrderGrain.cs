using System.Threading.Tasks;
using Contracts.Trading;
using Orleans;

namespace Abstractions
{
	public interface IOrderGrain : IGrainWithGuidCompoundKey
	{
		Task Receive(IOrder order);
		Task Update(IOrder order);
	}
}