using System.Threading.Tasks;
using Common.Trading;
using Orleans;

namespace Abstractions
{
	public interface IOrderProcessingGrain : IGrainWithGuidKey
	{
		Task Update(IOrder order);
	}
}