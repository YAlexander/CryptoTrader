using System.Threading.Tasks;
using Contracts.Trading;
using Orleans;

namespace Abstractions
{
	public interface IOrderProcessingGrain : IGrainWithGuidKey
	{
		Task Update(IOrder order);
	}
}