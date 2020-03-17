using System.Threading.Tasks;
using Common.Trading;
using Orleans;

namespace Abstractions
{
	public interface IOrderGrain : IGrainWithGuidCompoundKey
	{
		Task Receive(IOrder order);
		Task Update(IOrder order);
	}
}