using System.Threading.Tasks;
using Abstractions.Entities;
using Orleans;

namespace Abstractions.Grains
{
	public interface IOrderProcessingGrain : IGrainWithIntegerKey
	{
		Task Update(IOrder order);
	}
}